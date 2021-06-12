using System;
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
        public List<ServicePOCO> List_Services()
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
        public List<ServiceDetailPOCO> List_ServiceDetails(int serviceId)
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
        public bool? Validate_Part(int partId, short quantity)
        {
            using (var context = new BSMSContext())
            {
                var part = (from x in context.Parts
                              where x.PartID == partId
                              select x).FirstOrDefault();
                var partValidQuantity = (from x in context.Parts
                              where x.PartID == partId && x.QuantityOnHand >= quantity
                              select x).FirstOrDefault();
                bool? valid;

                if (part == null)
                {
                    valid = null;
                }
                else if (part != null && partValidQuantity == null)
                {
                    valid = false;
                }
                else
                {
                    valid = true;
                }

                return valid;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Part Lookup_Part(int partId)
        {
            using (var context = new BSMSContext())
            {
                Part part = (from x in context.Parts
                              where x.PartID == partId
                              select x).FirstOrDefault();

                return part;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ServiceDetailPartPOCO> List_ServiceDetailParts(int serviceDetailId)
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
        public void Add_Service(Job service, JobDetail serviceDetail)
        {
            using (var context = new BSMSContext())
            {
                context.Jobs.Add(service);
                context.JobDetails.Add(serviceDetail);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_ServiceDetail(JobDetail serviceDetail)
        {
            using (var context = new BSMSContext())
            {
                context.JobDetails.Add(serviceDetail);
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_ServiceDetailPart(int serviceDetailId, int partId, short quantity)
        {
            using (var context = new BSMSContext())
            {
                JobDetailPart existingServiceDetailPart = (from x in context.JobDetailParts
                              where x.JobDetailID == serviceDetailId && x.PartID == partId
                              select x).FirstOrDefault();
                Part inventoryPart = (from x in context.Parts
                              where x.PartID == partId
                              select x).FirstOrDefault();

                if (existingServiceDetailPart == null)
                {
                    //proceed to adding the new Service Detail Part since there's no existing part under the ServiceDetail
                    existingServiceDetailPart = new JobDetailPart();
                    existingServiceDetailPart.JobDetailID = serviceDetailId;
                    existingServiceDetailPart.PartID = partId;
                    existingServiceDetailPart.Quantity = quantity;
                    context.JobDetailParts.Add(existingServiceDetailPart);
                    //make the necessary update on the Parts Entity
                    inventoryPart.QuantityOnHand -= quantity;
                    context.Entry(inventoryPart).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //update just the quantity
                    existingServiceDetailPart.Quantity += quantity;
                    //make the necessary update on the Parts Entity
                    inventoryPart.QuantityOnHand -= quantity;
                    context.Entry(inventoryPart).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete_ServiceDetailPart(int serviceDetailPartID)
        {
            using (var context = new BSMSContext())
            {
                JobDetailPart serviceDetailPart = (from x in context.JobDetailParts
                                                  where x.JobDetailPartID == serviceDetailPartID
                                                  select x).FirstOrDefault();
                int partId = serviceDetailPart.PartID;
                short quantity = serviceDetailPart.Quantity;
                Part inventoryPart = (from y in context.Parts
                                      where y.PartID == partId
                                      select y).FirstOrDefault();

                //make the necessary update on the Parts Entity
                inventoryPart.QuantityOnHand += quantity;
                context.Entry(inventoryPart).State = System.Data.Entity.EntityState.Modified;

                context.JobDetailParts.Remove(serviceDetailPart);
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
        public void Update_ServiceDetail_AppendComment(int serviceDetailId,string existingComments, string inputComments)
        {
            using (var context = new BSMSContext())
            {
                var serviceDetail = (from x in context.JobDetails
                                     where x.JobDetailID == serviceDetailId
                                     select x).FirstOrDefault();

                //initialize string to capture merged existing and input comments
                string mergedComments = null;

                //add a semi-colon as a separator between comments if there's an existing comment
                if (string.IsNullOrWhiteSpace(existingComments))
                {
                    mergedComments += inputComments;
                }
                else
                {
                    mergedComments = existingComments + "; " + inputComments;
                }

                //apply update to service detail comments
                serviceDetail.Comments = mergedComments;
                context.Entry(serviceDetail).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Update_ServiceDetail_Status(int serviceId, int serviceDetailId, bool? newStatus)
        {
            using (var context = new BSMSContext())
            {
                var service = (from x in context.Jobs
                               where x.JobID == serviceId
                               select x).FirstOrDefault();
                var serviceDetail = (from x in context.JobDetails
                               where x.JobID == serviceId && x.JobDetailID == serviceDetailId
                               select x).FirstOrDefault();
                int startedServiceDetailCount = (from x in context.JobDetails
                                             where x.JobID == serviceId && x.Completed == false
                                             select x).Count();
                int serviceDetailCount = (from x in context.JobDetails
                                                 where x.JobID == serviceId
                                                 select x).Count();
                int finishedServiceDetailCount = (from x in context.JobDetails
                                                 where x.JobID == serviceId && x.Completed == true
                                                 select x).Count();

                //TO-DO: update quantity of parts used

                //check if service is starting its first service detail
                if (newStatus == false && startedServiceDetailCount == 0)
                {
                    //update service start date
                    service.JobDateStarted = DateTime.Now;
                }
                //else if the service is finishing all service details, including the service detail being updated
                else if (newStatus == true && serviceDetailCount == finishedServiceDetailCount + 1)
                {
                    service.JobDateDone = DateTime.Now;
                }

                //update service detail status
                serviceDetail.Completed = newStatus;

                //save changes to the database
                context.Entry(serviceDetail).State = System.Data.Entity.EntityState.Modified;
                context.Entry(service).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Update_ServiceDetailPart_Quantity(int serviceDetailPartId, short quantity)
        {
            using (var context = new BSMSContext())
            {
                var serviceDetailPart = (from x in context.JobDetailParts
                                         where x.JobDetailPartID == serviceDetailPartId
                                         select x).FirstOrDefault();

                //update service detail part quantity
                serviceDetailPart.Quantity = quantity;

                //save changes to the database
                context.Entry(serviceDetailPart).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
