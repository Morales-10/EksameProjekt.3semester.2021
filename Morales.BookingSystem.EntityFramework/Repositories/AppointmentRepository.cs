using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Morales.BookingSystem.Domain.IRepositories;
using Morales.BookingSystem.Domain.Services;
using Morales.BookingSystem.EntityFramework.Entities;

namespace Morales.BookingSystem.EntityFramework.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MainDbContext _ctx;

        public AppointmentRepository(MainDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<Appointment> readAllAppointments()
        {
            return _ctx.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.TreatmentsList)
                .ThenInclude(at => at.Treatment)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Customer = new Account{Id = ae.Customer.Id, Name = ae.Customer.Name, PhoneNumber = ae.Customer.PhoneNumber},
                    Employee = new Account{Id = ae.Employee.Id, Name = ae.Employee.Name},
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => te.Treatment != null ? new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    } : null).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .ToList();
        }

        public Appointment ReadById(int appointmentId)
        {
            return _ctx.Appointments.Where(ae => ae.Id == appointmentId)
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.TreatmentsList)
                .ThenInclude(at => at.Treatment)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Employeeid = ae.EmployeeId,
                    Customer = new Account{Id = ae.Customer.Id, Name = ae.Customer.Name, PhoneNumber = ae.Customer.PhoneNumber},
                    Employee = new Account{Id = ae.Employee.Id, Name = ae.Employee.Name},
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => te.Treatment != null ? new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    } : null).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .FirstOrDefault(ae => ae.Id == appointmentId);
        }

        public Appointment CreateAppointment(Appointment appointmentToCreate)
        {
            var entity = _ctx.Add(new AppointmentEntity()
            {
                CustomerId = appointmentToCreate.Customerid,
                EmployeeId = appointmentToCreate.Employeeid,
                Date = appointmentToCreate.Date,
                Duration = appointmentToCreate.Duration,
                TotalPrice = appointmentToCreate.TotalPrice,
                AppointmentEnd = appointmentToCreate.AppointmentEnd
            }).Entity;
            _ctx.SaveChanges();
            if (appointmentToCreate.TreatmentsList != null)
            {
                var appointmentTreatments = appointmentToCreate.TreatmentsList
                    .Select(t => new AppointmentTreatmentEntity
                    {
                        AppointmentId = entity.Id,
                        TreatmentId = t.Id
                    }).ToList();
                _ctx.AddRange(appointmentTreatments);
                _ctx.SaveChanges();
            }
            return new Appointment()
            {
                Id = entity.Id,
                Customerid = entity.CustomerId,
                Employeeid = entity.EmployeeId,
                Date = entity.Date,
                Duration = entity.Duration,
                AppointmentEnd = entity.AppointmentEnd
            } ;
        }

        public Appointment UpdateById(int appointmentToUpdateId, Appointment updatedAppointment)
        {
            var previousAppointment = _ctx.Appointments.Where(ae => ae.Id == appointmentToUpdateId)
                .Include(a => a.TreatmentsList)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Customerid = ae.CustomerId,
                    Employeeid = ae.EmployeeId,
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    }).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .FirstOrDefault(ae => ae.Id == appointmentToUpdateId);

            var appointmentEntity = new AppointmentEntity()
            {
                Id = previousAppointment.Id,
                CustomerId = previousAppointment.Customerid,
                EmployeeId = updatedAppointment.Employeeid,
                Date = updatedAppointment.Date,
                Duration = updatedAppointment.Duration,
                TotalPrice = updatedAppointment.TotalPrice,
                AppointmentEnd = updatedAppointment.AppointmentEnd
            };
            var entity = _ctx.Update(appointmentEntity).Entity;
            _ctx.SaveChanges();
            return new Appointment
            {
                Id = previousAppointment.Id,
                Customerid = previousAppointment.Customerid,
                Employeeid = updatedAppointment.Employeeid,
                Date = updatedAppointment.Date,
                Duration = updatedAppointment.Duration,
                TreatmentsList = updatedAppointment.TreatmentsList != null ? updatedAppointment.TreatmentsList.Select(te => new Treatments
                {
                    Id = te.Id,
                    Duration = te.Duration,
                    Name = te.Name,
                    Price = te.Price
                }).ToList() : null,
                TotalPrice = updatedAppointment.TotalPrice,
                AppointmentEnd = previousAppointment.AppointmentEnd
            };

        }

        public List<Appointment> GetAppointmentFromHairdresser(int employeeId)
        {
            return _ctx.Appointments.Where(ae => ae.EmployeeId== employeeId)
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.TreatmentsList)
                .ThenInclude(at => at.Treatment)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Customerid = ae.CustomerId,
                    Employeeid = ae.EmployeeId,
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    }).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .ToList();
        }

        public Appointment DeleteAppointment(int deletedAppointmentId)
        {
            var appointmentToDelete = _ctx.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.TreatmentsList)
                .ThenInclude(at => at.Treatment)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Customer = new Account{Id = ae.Customer.Id, Name = ae.Customer.Name, PhoneNumber = ae.Customer.PhoneNumber},
                    Employee = new Account{Id = ae.Employee.Id, Name = ae.Employee.Name},
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => te.Treatment != null ? new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    } : null).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .FirstOrDefault(a => a.Id == deletedAppointmentId);
            var OneDayFromNow = new DateTime();
            OneDayFromNow = DateTime.Now;
            OneDayFromNow = OneDayFromNow.AddHours(24);
            var diffrence = (appointmentToDelete.Date - OneDayFromNow).TotalHours;
            if (diffrence >= 0)
            {
                throw new Exception("You cannot delete an appointment less than 24 hours before");
            }
            _ctx.Appointments.Remove(new AppointmentEntity() {Id = deletedAppointmentId});
            _ctx.SaveChanges();
            return appointmentToDelete;
        }

        public List<Appointment> GetAppointmentFromUser(int userId)
        {
            return _ctx.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.TreatmentsList)
                .ThenInclude(at => at.Treatment)
                .Where(ae => ae.CustomerId== userId)
                .Select(ae => new Appointment
                {
                    Id = ae.Id,
                    Customerid = ae.CustomerId,
                    Employeeid = ae.EmployeeId,
                    Customer = new Account{Id = ae.Customer.Id, Name = ae.Customer.Name, PhoneNumber = ae.Customer.PhoneNumber},
                    Employee = new Account{Id = ae.Employee.Id, Name = ae.Employee.Name},
                    Date = ae.Date,
                    Duration = ae.Duration,
                    TreatmentsList = ae.TreatmentsList != null ? ae.TreatmentsList.Select(te => new Treatments
                    {
                        Id = te.Treatment.Id,
                        Duration = te.Treatment.Duration,
                        Name = te.Treatment.Name,
                        Price = te.Treatment.Price
                    }).ToList() : null,
                    TotalPrice = ae.TotalPrice,
                    AppointmentEnd = ae.AppointmentEnd
                })
                .ToList();
        }
    }
}