using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region AdditionalNamespaces
using BSMSData.Entities.Security;
using BSMSData.POCOs;
using BSMSSystem.BLL;
using BSMSSystem.BLL.Security;
using BSMSData.Entities;
using System.Web.UI.HtmlControls;
#endregion

public partial class Servicing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                if (!User.IsInRole(SecurityRoles.WebsiteAdmins) && !User.IsInRole(SecurityRoles.ServicingStaff))
                {
                    Response.Redirect("~/Account/Login.aspx");
                }

                //display user's full name
                //if (User.IsInRole(SecurityRoles.ServicingStaff))
                //{
                //    var sysmgr = new UserManager();

                //    UserFullNameLabel.Text = "Mechanic: " + sysmgr.Get_EmployeeFullName(User.Identity.Name);
                //}
                //else
                //{
                //    UserFullNameLabel.Text = "User: " + User.Identity.Name;
                //}
            }
        }
        //hide message user control
        MessageUserControl.Visible = false;
        ServiceDetailsPanel.Visible = false;
        ServiceDetailPartsPanel.Visible = false;
    }

    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.HandleDataBoundException(e);
    }

    #region Create New Service
    protected void Clear_AddNewServicesForm()
    {
        CustomerDropDownList.SelectedIndex = 0;
        ServiceVehicleIdentificationTextBox.Text = "";

        NewServiceModalServiceDetailDescriptionTextBox.Text = null;
        NewServiceModalServiceDetailHoursTextBox.Text = null;
        NewServiceModalCouponDropDownList.SelectedIndex = 0;
        NewServiceModalServiceDetailCommentsTextBox = null;

        //reset textbox styling if style was changed due to invalid data entry
        ServiceVehicleIdentificationTextBox.CssClass = "form-control";
        NewServiceModalServiceDetailDescriptionTextBox.CssClass = "form-control";
    }

    protected void AddNewServiceButton_Click(object sender, EventArgs e)
    {
        //at this point, client-side validation either passed or is compromised through html editing (string length html attribute can be modified through browser's inspect tool).
        //validate string length and post-back message user control showing errors in the form.
        if (ServiceVehicleIdentificationTextBox.Text.Length > 50)
        {
            MessageUserControl.ShowValidationError("Character limit exceeded.", "The provided vehicle identitfication number or model description exceeded the 50-character limit.");
            MessageUserControl.Visible = true;
            ServiceVehicleIdentificationTextBox.CssClass = "form-control is-invalid";
        }
        else if (NewServiceModalServiceDetailDescriptionTextBox.Text.Length > 100)
        {
            MessageUserControl.ShowValidationError("Character limit exceeded.", "The provided service detail description exceeded the 100-character limit.");
            MessageUserControl.Visible = true;
            NewServiceModalServiceDetailDescriptionTextBox.CssClass = "form-control is-invalid";
        }
        else
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                Job newService = new Job();
                newService.JobDateIn = DateTime.Now;
                //get customer ID from dropdown list
                newService.CustomerID = int.Parse(CustomerDropDownList.SelectedValue);

                //get the mechanic's details
                var usermgr = new UserManager();
                newService.EmployeeID = usermgr.Get_EmployeeID(User.Identity.Name);
                newService.ShopRate = 50;
                newService.StatusCode = "I";
                newService.VehicleIdentification = ServiceVehicleIdentificationTextBox.Text;

                //add the first service detail of the service
                JobDetail newServiceDetail = new JobDetail();
                newServiceDetail.JobID = newService.JobID;
                newServiceDetail.Description = NewServiceModalServiceDetailDescriptionTextBox.Text;
                if (NewServiceModalCouponDropDownList.SelectedValue == "0")
                {
                    newServiceDetail.CouponID = null;
                }
                else
                {
                    newServiceDetail.CouponID = int.Parse(NewServiceModalCouponDropDownList.SelectedValue);
                }
                newServiceDetail.JobHours = decimal.Parse(NewServiceModalServiceDetailHoursTextBox.Text);
                newServiceDetail.Comments = NewServiceModalServiceDetailCommentsTextBox.InnerText;

                //add the new service to the database
                ServiceController sysmgr = new ServiceController();
                sysmgr.Add_Service(newService, newServiceDetail);
                //refresh Listview
                ServicesListView.DataBind();

                //select the newly added service by selecting the last record in the ListView
                ServicesListView.SelectedIndex = ServicesListView.Items.Count - 1;
                ServicesListView_SelectedIndexChanged(sender, e);

                //clear form fields
                Clear_AddNewServicesForm();

            }, "Success", "New service added and is currently selected to be managed.");
        }
    }
    #endregion

    #region Manage Current Services
    protected void ServicesListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetServiceDetailsListView();
        ServiceDetailsPanel.Visible = true;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];

        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        string customer = (serviceRow.FindControl("CustomerNameLabel") as Label).Text;
        string vehicleIdentification = (serviceRow.FindControl("VehicleIdentificationLabel") as Label).Text;

        SelectedServiceIDLabel.Text = serviceId.ToString();
        SelectedServiceCustomerNameLabel.Text = customer;
        SelectedServiceVehicleIdentificationLabel.Text = vehicleIdentification;

        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ServicesListViewTitle.Text = "Service #" + serviceId + " Information";
        ServicesListViewSubtitle.Text = "View service #" + serviceId + " information and manage service details. Deselect service to return to current services list.";
        AddNewServiceModalButtonPanel.Visible = false;
    }

    protected void DeselectServiceButton_Click(object sender, EventArgs e)
    {
        //deselect currently selected item
        ServicesListView.SelectedIndex = -1;

        ServicesListViewTitle.Text = "Current Services";
        ServicesListViewSubtitle.Text = "Select a service to view or manage.";
        AddNewServiceModalButtonPanel.Visible = true;

        ResetServiceDetailsListView();
    }
    protected void ServicesListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (ServicesListView.SelectedIndex != -1)
        {
            if (e.Item.DisplayIndex != ServicesListView.SelectedIndex)
            {
                e.Item.Visible = false;
            }

            ServiceDetailsPanel.Visible = true;
        }
    }
    #endregion

    #region Manage Service Details
    protected void ResetServiceDetailsListView()
    {
        ServiceDetailPartsListView.SelectedIndex = -1;
        ServiceDetailPartsListView.EditIndex = -1;
        ServiceDetailPartsListView.DataSource = null;
        ServiceDetailPartsListView.DataBind();

        ServiceDetailsListView.SelectedIndex = -1;
        ServiceDetailsListView.EditIndex = -1;
        ServiceDetailsListView.DataSource = null;
        ServiceDetailsListView.DataBind();
    }

    protected void ResetServiceDetailsPanel()
    {
        SelectedServiceIDLabel.Text = null;
        SelectedServiceCustomerNameLabel.Text = null;
        SelectedServiceVehicleIdentificationLabel.Text = null;

        ResetServiceDetailsListView();
    }

    protected void ServiceDetailsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        //check if there's an item selected or being edited
        if ((ServiceDetailsListView.SelectedIndex != -1) || (ServiceDetailsListView.EditIndex != -1))
        {
            //hide items that are neither selected nor being edited
            if ((e.Item.DisplayIndex != ServiceDetailsListView.SelectedIndex) && (e.Item.DisplayIndex != ServiceDetailsListView.EditIndex))
            {
                e.Item.Visible = false;
            }
        }
        //else, if no item is being edited or selected...
        else
        {
            string currentServiceDetailStatus = (e.Item.FindControl("StatusLabel") as Label).Text.Trim().ToLower();
            Button serviceStatusUpdateButton = e.Item.FindControl("ServiceStatusButton") as Button;
            Button removeServiceButton = e.Item.FindControl("RemoveServiceDetailButton") as Button;

            //modify StatusUpdate button text depending on each service detail's current status
            switch (currentServiceDetailStatus)
            {
                case null:
                    serviceStatusUpdateButton.Text = "Start";
                    break;
                case "started":
                    serviceStatusUpdateButton.Text = "Finish";
                    serviceStatusUpdateButton.ToolTip = "Finish Service Detail";
                    removeServiceButton.Visible = false;
                    break;
                case "done":
                    serviceStatusUpdateButton.Visible = false;
                    removeServiceButton.Visible = false;
                    break;
                default:
                    serviceStatusUpdateButton.Text = "Start";
                    break;
            }
        }
    }

    protected void ServiceDetailsListView_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        ResetServiceDetailsListView();
        ServiceDetailsListView.SelectedIndex = e.NewSelectedIndex;
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ListViewItem selectedServiceDetail = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int selectedServiceDetailID = int.Parse((selectedServiceDetail.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string selectedServiceDetailDescription = (selectedServiceDetail.FindControl("DescriptionLabel") as Label).Text.Trim();

        List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(selectedServiceDetailID);

        SelectedServiceDetailDescriptionLabel.Text = selectedServiceDetailDescription;
        SelectedServiceDetailDescriptionLabel2.Text = selectedServiceDetailDescription;

        ServiceDetailPartsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
        ServiceDetailPartsListView.DataBind();
    }

    protected void ServiceDetailsListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        ResetServiceDetailsListView();
        ServiceDetailsListView.EditIndex = e.NewEditIndex;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ListViewItem serviceDetailRow = ServiceDetailsListView.EditItem;

        HtmlTextArea commentsTB = serviceDetailRow.FindControl("ServiceDetailCommentsTextArea") as HtmlTextArea;
        commentsTB.Focus();

        ServiceDetailPartsListView.DataSource = null;
        ServiceDetailPartsListView.DataBind();
    }

    protected void ServiceDetailsListView_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        ResetServiceDetailsListView();
        DeselectServiceDetailButton_Click(sender, e);
    }

    protected void DeselectServiceDetailButton_Click(object sender, EventArgs e)
    {
        ServiceDetailsPanel.Visible = true;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.SelectedIndex = -1;
        ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailsListView.DataBind();

        ServiceDetailPartsListView.DataSource = null;
        ServiceDetailPartsListView.DataBind();
    }

    protected void ClearServiceDetailInsertRowButton_Click(object sender, EventArgs e)
    {
        ListViewItem insertRow = ServiceDetailsListView.InsertItem;
        TextBox serviceDetailDescriptionTb = insertRow.FindControl("InsertRowServiceDetailDescriptionTextBox") as TextBox;
        serviceDetailDescriptionTb.Focus();
    }

    protected void ServiceDetailsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int i = e.Item.DisplayIndex;

        if ((e.CommandName.Equals("Edit")) || (e.CommandName.Equals("Select")))
        {
            ServiceDetailsListView.InsertItemPosition = 0;
            ServiceDetailsPanel.Visible = true;
        }
        else if (e.CommandName.Equals("Cancel"))
        {
            ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
            ServiceDetailsPanel.Visible = true;
        }
        else if (e.CommandName.Equals("UpdateStatus"))
        {
            ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
            ServiceDetailsPanel.Visible = true;

            //fetch data from service detail item
            ListViewItem serviceDetailRow = ServiceDetailsListView.Items[i];
            int serviceId = int.Parse((serviceDetailRow.FindControl("ServiceIDLabel") as Label).Text);
            int serviceDetailId = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
            string description = (serviceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();
            string status = (serviceDetailRow.FindControl("StatusLabel") as Label).Text.Trim();

            //fetch selected service row index for re-selection after data bind
            int serviceRowIndex = ServicesListView.SelectedIndex;

            //initialize variables needed
            bool? newStatus = null;
            string customSuccessMessage = "";

            if (string.IsNullOrWhiteSpace(status))
            {
                //set status to Started
                newStatus = false;
                customSuccessMessage = description + " service detail for service #" + serviceId + " has started.";
            }
            else if (status == "Started")
            {
                //set status to Done
                newStatus = true;
                customSuccessMessage = description + " service detail for service #" + serviceId + " has finished.";
            }

            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();

                //update service detail status and appropriate service dates
                sysmgr.Update_ServiceDetail_Status(serviceId, serviceDetailId, newStatus);

                //refresh current services list
                ServicesODS.DataBind();
                ServicesListView.DataBind();
                ServicesListView.SelectedIndex = serviceRowIndex;

                //refresh current service detail list
                List<ServiceDetailPOCO> detailResults = sysmgr.List_ServiceDetails(serviceId);
                ServiceDetailsListView.DataSource = detailResults;
                ServiceDetailsListView.DataBind();

            }, "Status Updated", customSuccessMessage);
        }
    }

    protected void ServiceDetailsListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int i = e.ItemIndex;

        //delete the service along with the only service detail associated with it
        if (ServiceDetailsListView.Items.Count == 1)
        {
            ListViewItem serviceDetailRow = ServiceDetailsListView.Items[i];
            int serviceDetailID = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
            int serviceID = int.Parse((serviceDetailRow.FindControl("ServiceIDLabel") as Label).Text);

            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();
                sysmgr.Delete_ServiceDetail(serviceDetailID);
                sysmgr.Delete_Service(serviceID);

                Clear_AddNewServicesForm();

                //refresh service detail
                ServiceDetailsListView.DataBind();
                ServiceDetailsPanel.Visible = false;
                //refresh current services list
                ServicesListView.SelectedIndex = -1;
                ServicesListView.DataBind();

            }, "Service Removed", "Service #" + serviceID + " had no more details and has been removed.");
        }
        //delete only the service detail
        else
        {
            ListViewItem serviceDetailRow = ServiceDetailsListView.Items[i];
            int serviceDetailID = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
            int serviceID = int.Parse((serviceDetailRow.FindControl("ServiceIDLabel") as Label).Text);

            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();
                sysmgr.Delete_ServiceDetail(serviceDetailID);

                Clear_AddNewServicesForm();

                //refresh current service details list
                ServiceDetailsListView.DataSource = sysmgr.List_ServiceDetails(serviceID);
                ServiceDetailsListView.DataBind();

                //keep showing service details panel
                ServiceDetailsPanel.Visible = true;

            }, "Service Detail Removed", "Service detail has been removed from service #" + serviceID + ".");
        }
    }

    protected void ServiceDetailsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.TryRun(() =>
        {
            //fetch Service ID from selected service row
            ListViewItem selectedServiceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
            int selectedServiceID = int.Parse((selectedServiceRow.FindControl("ServiceIDLabel") as Label).Text);

            //fetch entered data in the insert row
            ListViewItem insertServiceDetailRow = e.Item;
            string description = (insertServiceDetailRow.FindControl("InsertRowServiceDetailDescriptionTextBox") as TextBox).Text.Trim();
            decimal hours = decimal.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailHoursTextBox") as TextBox).Text.Trim());
            int couponId = int.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailCouponDropDownList") as DropDownList).SelectedValue.Trim());

            HtmlTextArea commentsTextArea = insertServiceDetailRow.FindControl("ServiceDetailCommentsTextArea") as HtmlTextArea;
            string comments = commentsTextArea.InnerText.Trim();
            //string comments = (insertServiceDetailRow.FindControl("InsertRowServiceDetailCommentsTextBox") as TextBox).Text.Trim();

            //add the service detail to the selected service
            JobDetail newServiceDetail = new JobDetail();
            newServiceDetail.JobID = selectedServiceID;
            newServiceDetail.Description = description;
            newServiceDetail.JobHours = hours;
            if (couponId == 0)
            {
                newServiceDetail.CouponID = null;
            }
            else
            {
                newServiceDetail.CouponID = couponId;
            }
            newServiceDetail.Comments = comments;

            //add the new service detail to the database
            ServiceController sysmgr = new ServiceController();
            sysmgr.Add_ServiceDetail(newServiceDetail);
            //refresh Listview
            ServicesListView_SelectedIndexChanged(sender, e);

            //clear form fields
            Clear_AddNewServicesForm();

        }, "Success", "New service detail added to selected service.");
    }

    protected void ServiceDetailsListView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailsPanel.Visible = true;

        ListViewItem editRow = ServiceDetailsListView.Items[e.ItemIndex];

        int serviceId = int.Parse((editRow.FindControl("ServiceIDLabel") as Label).Text);
        int serviceDetailId = int.Parse((editRow.FindControl("ServiceDetailIDLabel") as Label).Text);
        string existingComments = (editRow.FindControl("ServiceDetailCommentsLabel") as Label).Text.Trim();
        string inputComments = (editRow.FindControl("ServiceDetailCommentsTextArea") as HtmlTextArea).InnerText.Trim();

        MessageUserControl.Visible = true;
        MessageUserControl.TryRun(() =>
        {
            ServiceController sysmgr = new ServiceController();
            sysmgr.Update_ServiceDetail_AppendComment(serviceDetailId, existingComments, inputComments);

            //refresh current job detail list
            List<ServiceDetailPOCO> detailResults = sysmgr.List_ServiceDetails(serviceId);

            ServiceDetailsListView.DataSource = detailResults;
            ServiceDetailsListView.EditIndex = -1;
            ServiceDetailsListView.DataBind();
        }, "Comment Append Successful", "Additional comments have been added to any existing comments in the currently-selected service detail.");
    }
    #endregion

    #region Manage Service Detail Parts
    protected void ServiceDetailPartsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        //if there are no items being edited
        if (ServiceDetailPartsListView.EditIndex != -1)
        {
            //hide items that are not being edited
            if (e.Item.DisplayIndex != ServiceDetailPartsListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }

    protected void ServiceDetailPartsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        //get Service detail ID from currently-selected service detail
        ListViewItem selectedServiceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int serviceDetailID = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string serviceDescription = (selectedServiceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();

        //fetch input in the CurrentServiceDetailPartsListView insert row
        ListViewItem serviceDetailPartInsertRow = e.Item;
        TextBox insertPartIdTb = serviceDetailPartInsertRow.FindControl("PartIDTextBox") as TextBox;
        TextBox insertQuantityTb = serviceDetailPartInsertRow.FindControl("QuantityTextBox") as TextBox;
        int insertPartID = int.Parse(insertPartIdTb.Text.Trim());
        short insertQuantity = short.Parse(insertQuantityTb.Text.Trim());

        //validate if the entered part exists
        ServiceController sysmgr = new ServiceController();
        bool? insertingPartIsValid = sysmgr.Validate_Part(insertPartID, insertQuantity);

        MessageUserControl.Visible = true;

        if (insertingPartIsValid == null)
        {
            MessageUserControl.ShowValidationError("Invalid Part Number", "Provided part number " + insertPartID + " was not found or does not exist in the parts inventory.");
            insertPartIdTb.Focus();
            insertPartIdTb.CssClass = "form-control is-invalid";
        }
        else if (insertingPartIsValid == false)
        {
            Part foundPart = sysmgr.Lookup_Part(insertPartID);
            string foundPartDescription = foundPart.Description;
            int quantityOnHand = foundPart.QuantityOnHand;
            MessageUserControl.ShowValidationError("Invalid Quantity", "Not enough " + foundPartDescription + " in stock. Quantity must be " + quantityOnHand + " or less.");
            insertQuantityTb.Focus();
            insertQuantityTb.CssClass = "form-control is-invalid";
        }
        else
        {
            Part foundPart = sysmgr.Lookup_Part(insertPartID);
            string foundPartDescription = foundPart.Description;
            int quantityOnHand = foundPart.QuantityOnHand;

            MessageUserControl.TryRun(() =>
            {
                sysmgr.Add_ServiceDetailPart(serviceDetailID, insertPartID, insertQuantity);

                //refresh Service Detail Parts ListView
                List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(serviceDetailID);
                ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
                ServiceDetailPartsListView.DataBind();
                insertPartIdTb.Focus();

            }, "Service Detail Part added", foundPartDescription + " has been added to the "+ serviceDescription + " service.");
        }

        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;
    }

    protected void ServiceDetailPartsListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        ServiceDetailPartsListView.EditIndex = e.NewEditIndex;
        ServiceDetailPartsListView.InsertItemPosition = 0;
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;

        ListViewItem serviceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int serviceDetailId = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(serviceDetailId);
        ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
        ServiceDetailPartsListView.DataBind();

        ListViewItem serviceDetailPartRow = ServiceDetailPartsListView.EditItem;

        var quantityTB = serviceDetailPartRow.FindControl("QuantityTextBox");
        quantityTB.Focus();
    }

    protected void ServiceDetailPartsListView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        ServiceDetailPartsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;

        //ListViewItem editRow = ServiceDetailPartsListView.Items[e.ItemIndex];
        ListViewItem editRow = ServiceDetailPartsListView.EditItem;

        int serviceDetailPartId = int.Parse((editRow.FindControl("ServiceDetailPartIDLabel") as Label).Text);
        int serviceDetailId = int.Parse((editRow.FindControl("ServiceDetailIDLabel") as Label).Text);
        short newQuantity = short.Parse((editRow.FindControl("QuantityTextBox") as TextBox).Text);

        MessageUserControl.Visible = true;
        MessageUserControl.TryRun(() =>
        {
            ServiceController sysmgr = new ServiceController();
            sysmgr.Update_ServiceDetailPart_Quantity(serviceDetailPartId, newQuantity);

            //refresh current service detail parts list
            List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);

            ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
            ServiceDetailPartsListView.EditIndex = -1;
            ServiceDetailPartsListView.DataBind();
        }, "Success", "Quantity updated.");
    }

    protected void ServiceDetailPartsListView_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {

        ServiceController sysmgr = new ServiceController();
        //if there's an item being edited, and the cancel button is clicked,
        if (ServiceDetailPartsListView.EditIndex != -1)
        {
            ListViewItem editRow = ServiceDetailPartsListView.EditItem;
            (editRow.FindControl("QuantityTextBox") as TextBox).Text = null;
            ServiceDetailPartsListView.EditIndex = -1;
            int serviceDetailId = int.Parse((editRow.FindControl("ServiceDetailIDLabel") as Label).Text);

            //refresh current service detail parts list
            List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);
            ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
        }
        //else, if the cancel/clear button in the insert row is clicked,
        else
        {
            ListViewItem insertRow = ServiceDetailPartsListView.InsertItem;
            (insertRow.FindControl("PartIDTextBox") as TextBox).Text = null;
            (insertRow.FindControl("QuantityTextBox") as TextBox).Text = null;

            ListViewItem selectedServiceDetail = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
            int serviceDetailId = int.Parse((selectedServiceDetail.FindControl("ServiceDetailIDLabel") as Label).Text);

            //refresh current service detail parts list
            List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);
            ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
        }

        ServiceDetailPartsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailPartsListView.DataBind();
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;
    }

    protected void ServiceDetailPartsListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        ListViewItem serviceDetailPartRow = ServiceDetailPartsListView.Items[e.ItemIndex];
        int serviceDetailPartID = int.Parse((serviceDetailPartRow.FindControl("ServiceDetailPartIDLabel") as Label).Text);

        MessageUserControl.TryRun(() =>
        {
            //get Service detail ID from currently-selected service detail
            ListViewItem selectedServiceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
            int serviceDetailID = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);

            ServiceController sysmgr = new ServiceController();
            sysmgr.Delete_ServiceDetailPart(serviceDetailPartID);

            //refresh Service Detail Parts ListView
            List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(serviceDetailID);
            ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
            ServiceDetailPartsListView.DataBind();

            ServiceDetailsPanel.Visible = true;
            ServiceDetailPartsPanel.Visible = true;
            MessageUserControl.Visible = true;
        }, "Service Detail Part Removed", "Service detail part has been removed from current service detail.");
    }
    #endregion
}