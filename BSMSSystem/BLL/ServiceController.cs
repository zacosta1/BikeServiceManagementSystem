﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using System.ComponentModel;
using BSMSData.POCOs;
using BSMSSystem.DAL;
using BSMSData.Entities;
using BSMSData.DTOs;
#endregion

namespace BSMSSystem.BLL
{
    [DataObject]
    public class ServiceController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ServiceDTO> List_ServiceDetailParts()
        {
            using (var context = new BSMSContext())
            {
                var results = from x in context.Jobs
                              where x.JobDateOut == null
                              orderby x.JobDateIn
                              select new ServiceDTO
                              {
                                  ServiceID = x.JobID,
                                  In = x.JobDateIn,
                                  Started = x.JobDateStarted,
                                  Done = x.JobDateDone,
                                  CustomerID = x.CustomerID,
                                  CustomerName = x.Customer.LastName + ", " + x.Customer.FirstName,
                                  ContactNumber = x.Customer.ContactPhone,
                                  VehicleIdentification = x.VehicleIdentification,
                                  ServiceDetails = from y in x.JobDetails
                                                   select new ServiceDetailDTO
                                                   {
                                                       ServiceDetailID = y.JobDetailID,
                                                       ServiceID = y.JobID,
                                                       Description = y.Description,
                                                       ServiceDetailHours = y.JobHours,
                                                       Coupon = y.Coupon.CouponIDValue,
                                                       Comments = y.Comments,
                                                       Status = y.Completed == false ? "Started" :
                                                                y.Completed == true ? "Done" :
                                                                null,
                                                       ServiceDetailParts = from z in y.JobDetailParts
                                                                            select new ServiceDetailPartPOCO
                                                                            {
                                                                                ServiceDetailID = z.JobDetailID,
                                                                                ServiceDetailPartID = z.JobDetailPartID,
                                                                                PartID = z.PartID,
                                                                                PartDescription = z.Part.Description,
                                                                                Quantity = z.Quantity
                                                                            }
                                                   }
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ServicePOCO> List_CurrentServices()
        {
            using (var context = new BSMSContext())
            {
                var results = from x in context.Jobs
                              where x.JobDateOut == null
                              select new ServicePOCO
                              {
                                  ServiceID = x.JobID,
                                  In = x.JobDateIn,
                                  Started = x.JobDateStarted,
                                  Done = x.JobDateDone,
                                  CustomerID = x.CustomerID,
                                  CustomerName = x.Customer.LastName + ", " + x.Customer.FirstName,
                                  ContactNumber = x.Customer.ContactPhone,
                                  VehicleIdentification = x.VehicleIdentification
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ServiceDetailPOCO> List_ServiceDetailsByServiceID(int serviceId)
        {
            using (var context = new BSMSContext())
            {
                var results = from x in context.JobDetails
                              where x.JobID == serviceId
                              orderby x.JobHours ascending
                              select new ServiceDetailPOCO
                              {
                                  ServiceDetailID = x.JobDetailID,
                                  ServiceID = x.JobID,
                                  Description = x.Description,
                                  ServiceDetailHours = x.JobHours,
                                  Coupon = x.Coupon.CouponIDValue,
                                  Comments = x.Comments,
                                  Status = x.Completed == false ? "Started" :
                                            x.Completed == true ? "Done" :
                                            null
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<StandardJob> List_StandardServiceDetails()
        {
            using (var context = new BSMSContext())
            {
                return context.StandardJobs.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Coupon> List_Coupons()
        {
            using (var context = new BSMSContext())
            {
                return context.Coupons.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Part> List_Parts()
        {
            using (var context = new BSMSContext())
            {
                return context.Parts.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ServiceDetailPartPOCO> List_CurrentServiceDetailPartsByServiceDetailID(int serviceDetailId)
        {
            using (var context = new BSMSContext())
            {
                var results = from x in context.JobDetailParts
                              where x.JobDetailID == serviceDetailId
                              orderby x.JobDetail.JobHours ascending
                              select new ServiceDetailPartPOCO
                              {
                                  ServiceDetailID = x.JobDetailID,
                                  ServiceDetailPartID = x.JobDetailPartID,
                                  PartID = x.PartID,
                                  PartDescription = x.Part.Description,
                                  Quantity = x.Quantity
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_NewService(Job service, JobDetail serviceDetail)
        {
            using (var context = new BSMSContext())
            {
                context.Jobs.Add(service);
                context.JobDetails.Add(serviceDetail);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_NewServiceDetail(JobDetail serviceDetail)
        {
            using (var context = new BSMSContext())
            {
                context.JobDetails.Add(serviceDetail);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_NewServiceDetailPart(int serviceDetailId, int partId, short quantity)
        {
            using (var context = new BSMSContext())
            {
                var exists = (from x in context.JobDetailParts
                              where x.JobDetailID == serviceDetailId && x.PartID == partId
                              select x).FirstOrDefault();

                if (exists == null)
                {
                    //proceed to adding the new Service Detail Part since there's no existing part under the ServiceDetail
                    exists = new JobDetailPart();
                    exists.JobDetailID = serviceDetailId;
                    exists.PartID = partId;
                    exists.Quantity = quantity;
                    context.JobDetailParts.Add(exists);
                }
                else
                {
                    //update just the quantity
                    exists.Quantity += quantity;
                }

                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete_ServiceDetailPart(int serviceDetailPartID)
        {
            using (var context = new BSMSContext())
            {
                JobDetailPart seviceDetailPart = (from x in context.JobDetailParts
                                                  where x.JobDetailPartID == serviceDetailPartID
                                                  select x).FirstOrDefault();
                context.JobDetailParts.Remove(seviceDetailPart);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete_ServiceDetail(int serviceDetailID)
        {
            using (var context = new BSMSContext())
            {
                List<JobDetailPart> serviceDetailParts = (from x in context.JobDetailParts
                                                          where x.JobDetailID == serviceDetailID
                                                          select x).ToList();
                JobDetail serviceDetail = (from x in context.JobDetails
                                           where x.JobDetailID == serviceDetailID
                                           select x).FirstOrDefault();
                context.JobDetailParts.RemoveRange(serviceDetailParts);
                context.JobDetails.Remove(serviceDetail);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete_Service(int serviceID)
        {
            using (var context = new BSMSContext())
            {
                Job service = (from x in context.Jobs
                               where x.JobID == serviceID
                               select x).FirstOrDefault();
                context.Jobs.Remove(service);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int Update_ServiceDetail(JobDetail item)
        {
            using (var context = new BSMSContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int Update_ServiceDetailStatus(ServiceDetailPOCO item)
        {
            using (var context = new BSMSContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }
    }
}