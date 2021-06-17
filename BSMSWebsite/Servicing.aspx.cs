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
using System.Globalization;
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
        if (ServiceVehicleIdentificationTextBox.Text.Trim().Length > 50)
        {
            MessageUserControl.ShowValidationError("Character limit exceeded.", "The provided vehicle identitfication number or model description exceeded the 50-character limit.");
            MessageUserControl.Visible = true;
            ServiceVehicleIdentificationTextBox.CssClass = "form-control is-invalid";
        }
        else if (NewServiceModalServiceDetailDescriptionTextBox.Text.Trim().Length > 100)
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
                newService.VehicleIdentification = ServiceVehicleIdentificationTextBox.Text.Trim();

                //add the first service detail of the service
                JobDetail newServiceDetail = new JobDetail();
                newServiceDetail.JobID = newService.JobID;

                // Creates a TextInfo based on the "en-US" culture.
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                newServiceDetail.Description = textInfo.ToTitleCase(NewServiceModalServiceDetailDescriptionTextBox.Text.Trim());

                if (NewServiceModalCouponDropDownList.SelectedValue == "0")
                {
                    newServiceDetail.CouponID = null;
                }
                else
                {
                    newServiceDetail.CouponID = int.Parse(NewServiceModalCouponDropDownList.SelectedValue);
                }
                newServiceDetail.JobHours = decimal.Parse(NewServiceModalServiceDetailHoursTextBox.Text.Trim());
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

        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text.Trim());
        string customer = (serviceRow.FindControl("CustomerNameLabel") as Label).Text.Trim();
        string vehicleIdentification = (serviceRow.FindControl("VehicleIdentificationLabel") as Label).Text.Trim();

        SelectedServiceIDLabel.Text = serviceId.ToString().Trim();
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

            //change StatusUpdate button text and visibility, and RemoveServiceDetail button visibility, depending on each service detail's current status
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
        ServiceDetailsListView.SelectedIndex = e.NewSelectedIndex;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ListViewItem selectedServiceDetail = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int selectedServiceDetailId = int.Parse((selectedServiceDetail.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string selectedServiceDetailDescription = (selectedServiceDetail.FindControl("DescriptionLabel") as Label).Text.Trim();

        List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(selectedServiceDetailId);

        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        SelectedServiceDetailDescriptionLabel.Text = textInfo.ToTitleCase(selectedServiceDetailDescription);
        SelectedServiceDetailDescriptionLabel2.Text = textInfo.ToTitleCase(selectedServiceDetailDescription);

        ServiceDetailPartsPanel.Visible = true;
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
        ServiceDetailsPanel.Visible = true;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetails(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;

        //if cancelling edit-mode
        if (ServicesListView.EditIndex != -1)
        {
            ServiceDetailsListView.EditIndex = -1;
            ServiceDetailsListView.DataBind();
        }
        //else clear insert row
        else
        {
            ServiceDetailsListView.DataBind();
            ListViewItem insertRow = ServiceDetailsListView.InsertItem;
            TextBox serviceDetailDescriptionTb = insertRow.FindControl("InsertRowServiceDetailDescriptionTextBox") as TextBox;
            serviceDetailDescriptionTb.Focus();
        }

        ServiceDetailPartsListView.DataSource = null;
        ServiceDetailPartsListView.DataBind();
    }

    protected void DeselectServiceDetailButton_Click(object sender, EventArgs e)
    {
        ResetServiceDetailsListView();
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

    protected void ServiceDetailsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int i = e.Item.DisplayIndex;

        if (e.CommandName.Equals("Edit") || e.CommandName.Equals("Select"))
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

        ListViewItem serviceDetailRow = ServiceDetailsListView.Items[i];
        int serviceDetailId = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
        string serviceDetailDescription = (serviceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();
        int serviceId = int.Parse((serviceDetailRow.FindControl("ServiceIDLabel") as Label).Text);

        MessageUserControl.Visible = true;
        //delete the service along with the only service detail associated with it
        if (ServiceDetailsListView.Items.Count == 1)
        {
            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();
                sysmgr.Delete_ServiceDetail(serviceDetailId);
                sysmgr.Delete_Service(serviceId);

                Clear_AddNewServicesForm();

                //refresh service detail
                ServiceDetailsListView.DataBind();
                ServiceDetailsPanel.Visible = false;
                //refresh current services list
                ServicesListView.SelectedIndex = -1;
                ServicesListView.DataBind();

            }, "Service Cancelled", "Service #" + serviceId + " had no more service details and has been cancelled.");
        }
        //delete only the service detail
        else
        {
            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();
                sysmgr.Delete_ServiceDetail(serviceDetailId);

                Clear_AddNewServicesForm();
                //refresh Listview
                ServicesListView.DataBind();

                //refresh current service details list
                ServiceDetailsListView.DataSource = sysmgr.List_ServiceDetails(serviceId);
                ServiceDetailsListView.DataBind();

                //keep showing service details panel
                ServiceDetailsPanel.Visible = true;

            }, "Service Detail Cancelled", serviceDetailDescription + " service detail has been cancelled from service #" + serviceId + ".");
        }
    }

    protected void ServiceDetailsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        //fetch Service ID from selected service row
        ListViewItem selectedServiceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int selectedServiceId = int.Parse((selectedServiceRow.FindControl("ServiceIDLabel") as Label).Text.Trim());

        //fetch entered data in the insert row
        ListViewItem insertServiceDetailRow = e.Item;
        TextBox descriptionTextBox = insertServiceDetailRow.FindControl("InsertRowServiceDetailDescriptionTextBox") as TextBox;

        string description = descriptionTextBox.Text.Trim();
        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        textInfo.ToTitleCase(description);
        decimal hours = decimal.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailHoursTextBox") as TextBox).Text.Trim());
        int couponId = int.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailCouponDropDownList") as DropDownList).SelectedValue.Trim());

        HtmlTextArea commentsTextArea = insertServiceDetailRow.FindControl("ServiceDetailCommentsTextArea") as HtmlTextArea;
        string comments = commentsTextArea.InnerText.Trim();

        //at this point, client-side validation either passed or is compromised through html editing (string length html attribute can be modified through browser's inspect tool).
        //validate string length and post-back message user control showing errors in the form.

        MessageUserControl.Visible = true;
        if (description.Length > 100)
        {
            MessageUserControl.ShowValidationError("Character limit exceeded.", "The provided service detail description exceeded the 100-character limit.");
            descriptionTextBox.CssClass = "form-control is-invalid";
            descriptionTextBox.Focus();

            ServiceDetailsPanel.Visible = true;
        }
        else
        {
            MessageUserControl.TryRun(() =>
            {
                //add the service detail to the selected service
                JobDetail newServiceDetail = new JobDetail();
                newServiceDetail.JobID = selectedServiceId;

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
                ServicesListView.DataBind();
                ServicesListView_SelectedIndexChanged(sender, e);

                //clear form fields
                Clear_AddNewServicesForm();

            }, "Success", description + " service has been added to service #" + selectedServiceId + ".");
        }
    }

    protected void ServiceDetailsListView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailsPanel.Visible = true;

        ListViewItem editRow = ServiceDetailsListView.Items[e.ItemIndex];

        int serviceId = int.Parse((editRow.FindControl("ServiceIDLabel") as Label).Text);
        int serviceDetailId = int.Parse((editRow.FindControl("ServiceDetailIDLabel") as Label).Text);
        string serviceDetailDescription = (editRow.FindControl("ServiceDetailDescriptionLabel") as Label).Text.Trim();
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
        }, "Comment Append Successful", "Additional comments have been added to any existing comments in the " + serviceDetailDescription + " service detail.");
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
        int serviceDetailId = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string serviceDetailDescription = (selectedServiceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();

        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //convert to title case
        textInfo.ToTitleCase(serviceDetailDescription);

        //fetch input in the CurrentServiceDetailPartsListView insert row
        ListViewItem serviceDetailPartInsertRow = e.Item;
        TextBox insertPartIdTb = serviceDetailPartInsertRow.FindControl("PartIDTextBox") as TextBox;
        TextBox insertQuantityTb = serviceDetailPartInsertRow.FindControl("QuantityTextBox") as TextBox;
        int insertPartId = int.Parse(insertPartIdTb.Text.Trim());
        short insertQuantity = short.Parse(insertQuantityTb.Text.Trim());

        //validate if the entered part exists
        ServiceController sysmgr = new ServiceController();
        bool? insertingPartIsValid = sysmgr.Validate_Part(insertPartId, insertQuantity);

        MessageUserControl.Visible = true;

        if (insertingPartIsValid == null)
        {
            MessageUserControl.ShowValidationError("Invalid Part Number", "Provided part #" + insertPartId + " was not found or does not exist in the parts inventory.");
            insertPartIdTb.Focus();
            insertPartIdTb.CssClass = "form-control is-invalid";
        }
        else if (insertingPartIsValid == false)
        {
            Part foundPart = sysmgr.Lookup_Part(insertPartId);
            string partDescription = foundPart.Description;
            int quantityOnHand = foundPart.QuantityOnHand;
            MessageUserControl.ShowValidationError("Invalid Quantity", "Not enough " + partDescription + " in stock. Quantity must be " + quantityOnHand + " or less.");
            insertQuantityTb.Focus();
            insertQuantityTb.CssClass = "form-control is-invalid";
        }
        else if (insertingPartIsValid == true)
        {
            Part foundPart = sysmgr.Lookup_Part(insertPartId);
            string partDescription = foundPart.Description;
            int quantityOnHand = foundPart.QuantityOnHand;

            MessageUserControl.TryRun(() =>
            {
                sysmgr.Add_ServiceDetailPart(serviceDetailId, insertPartId, insertQuantity);

                //refresh Service Detail Parts ListView
                List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(serviceDetailId);
                ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
                ServiceDetailPartsListView.DataBind();
                insertPartIdTb.Focus();

            }, "Service Detail Part Added", partDescription + " (x"+ insertQuantity + ") have been added to the "+ serviceDetailDescription + " service.");
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

        //get Service Detail ID and description from currently-selected service detail
        ListViewItem selectedServiceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int serviceDetailId = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string serviceDetailDescription = (selectedServiceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();

        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //convert to title case
        textInfo.ToTitleCase(serviceDetailDescription);

        //fetch edit row input
        ListViewItem editRow = ServiceDetailPartsListView.EditItem;
        TextBox editPartQuantityTb = editRow.FindControl("QuantityTextBox") as TextBox;

        int serviceDetailPartId = int.Parse((editRow.FindControl("ServiceDetailPartIDLabel") as Label).Text.Trim());
        short newQuantity = short.Parse(editPartQuantityTb.Text.Trim());
        int inventoryPartId = short.Parse((editRow.FindControl("PartIDLabel") as Label).Text.Trim());

        //validate if the entered part exists
        ServiceController sysmgr = new ServiceController();
        bool? newQuantityIsValid = sysmgr.Validate_Part_Quantity(serviceDetailPartId, inventoryPartId, newQuantity);

        MessageUserControl.Visible = true;
        if (newQuantityIsValid == null)
        {
            //refresh current service detail parts list
            List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);

            ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
            ServiceDetailPartsListView.EditIndex = e.ItemIndex;
            ServiceDetailPartsListView.InsertItemPosition = 0;

            Part foundPart = sysmgr.Lookup_Part(inventoryPartId);
            string partDescription = foundPart.Description;
            int quantityOnOrder = foundPart.QuantityOnOrder;
            MessageUserControl.ShowValidationError("No Changes Were Made", "The " + serviceDetailDescription + " service already uses/used the same quantity of " + partDescription + " as the newly-entered quantity value.");
            editPartQuantityTb.CssClass = "form-control is-invalid";
            editPartQuantityTb.Focus();
        }
        else if (newQuantityIsValid == false)
        {
            //refresh current service detail parts list
            List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);

            ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
            ServiceDetailPartsListView.EditIndex = e.ItemIndex;
            ServiceDetailPartsListView.InsertItemPosition = 0;

            Part foundPart = sysmgr.Lookup_Part(inventoryPartId);
            string partDescription = foundPart.Description;
            int quantityOnOrder = foundPart.QuantityOnOrder;
            MessageUserControl.ShowValidationError("Invalid Quantity", "Not enough " + partDescription + " left in stock for the " + serviceDetailDescription + " service to use. " + quantityOnOrder + " " + partDescription + " are currently on order.");
            editPartQuantityTb.CssClass = "form-control is-invalid";
            editPartQuantityTb.Focus();
        }
        else
        {
            Part foundPart = sysmgr.Lookup_Part(inventoryPartId);
            string partDescription = foundPart.Description;
            int quantityOnHand = foundPart.QuantityOnHand;
            MessageUserControl.TryRun(() =>
            {
                sysmgr.Update_ServiceDetailPart_Quantity(serviceDetailPartId, newQuantity);

                //refresh current service detail parts list
                List<ServiceDetailPartPOCO> serviceDetailPartsResults = sysmgr.List_ServiceDetailParts(serviceDetailId);

                ServiceDetailPartsListView.DataSource = serviceDetailPartsResults;
                ServiceDetailPartsListView.EditIndex = -1;
                ServiceDetailPartsListView.DataBind();
            }, "Quantity Up-to-date", "Quantity of " + partDescription + " used/needed in the " + serviceDetailDescription + " service has been updated.");
        }
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
            TextBox partIdTextBox = insertRow.FindControl("PartIDTextBox") as TextBox;
            partIdTextBox.Text = null;
            partIdTextBox.Focus();
            TextBox quantityTextBox = insertRow.FindControl("QuantityTextBox") as TextBox;
            quantityTextBox.Text = null;

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
        //get Service detail info from currently-selected service detail
        ListViewItem selectedServiceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int serviceDetailId = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string serviceDetailDescription = (selectedServiceDetailRow.FindControl("DescriptionLabel") as Label).Text.Trim();

        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //convert to title case
        textInfo.ToTitleCase(serviceDetailDescription);

        ListViewItem serviceDetailPartRow = ServiceDetailPartsListView.Items[e.ItemIndex];
        int serviceDetailPartId = int.Parse((serviceDetailPartRow.FindControl("ServiceDetailPartIDLabel") as Label).Text);
        int partId = int.Parse((serviceDetailPartRow.FindControl("PartIDLabel") as Label).Text.Trim());

        ServiceController sysmgr = new ServiceController();
        Part foundPart = sysmgr.Lookup_Part(partId);
        string partDescription = foundPart.Description;

        MessageUserControl.Visible = true;
        MessageUserControl.TryRun(() =>
        {
            sysmgr.Delete_ServiceDetailPart(serviceDetailPartId);

            //refresh Service Detail Parts ListView
            List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_ServiceDetailParts(serviceDetailId);
            ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
            ServiceDetailPartsListView.DataBind();

            ServiceDetailsPanel.Visible = true;
            ServiceDetailPartsPanel.Visible = true;
        }, "Service Detail Part Removed", partDescription + " has been removed from the " + serviceDetailDescription  + " service detail.");
    }
    #endregion
}