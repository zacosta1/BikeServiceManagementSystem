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

    protected void ResetServiceDetailsPanel()
    {
        SelectedServiceIDLabel.Text = null;
        SelectedServiceCustomerNameLabel.Text = null;
        SelectedServiceVehicleIdentificationLabel.Text = null;

        ResetServiceDetailsListView();
    }

    #region Create New Service

    protected void ClearNewServicesForm()
    {
        CustomerDropDownList.SelectedIndex = 0;
        ServiceVehicleIdentificationTextBox.Text = "";

        NewServiceModalServiceDetailDescriptionTextBox.Text = null;
        NewServiceModalServiceDetailHoursTextBox.Text = null;
        NewServiceModalCouponDropDownList.SelectedIndex = 0;
        NewServiceModalServiceDetailCommentsTextBox = null;
    }

    protected void AddNewServiceButton_Click(object sender, EventArgs e)
    {
        //check if a customer has been selected
        if (CustomerDropDownList.SelectedValue == "0")
        {
            MessageUserControl.ShowValidationError("Missing information", "Please select a customer.");
            CustomerDropDownList.Focus();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(ServiceVehicleIdentificationTextBox.Text))
            {
                MessageUserControl.ShowValidationError("Missing information", "Please provide the vehicle's identitfication number, model name, or model number.");
            }
            else if (string.IsNullOrWhiteSpace(NewServiceModalServiceDetailDescriptionTextBox.Text))
            {
                MessageUserControl.ShowValidationError("Missing information", "Please provide a description for this service.");
            }
            else if (string.IsNullOrEmpty(NewServiceModalServiceDetailHoursTextBox.Text))
            {
                MessageUserControl.ShowValidationError("Missing information", "Please indicate how many hours this service will take.");
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
                    sysmgr.Add_NewService(newService, newServiceDetail);
                    //refresh Listview
                    ServicesListView.DataBind();

                    //select the newly added service by selecting the last record in the ListView
                    ServicesListView.SelectedIndex = ServicesListView.Items.Count - 1;
                    ServicesListView_SelectedIndexChanged(sender, e);

                    //clear form fields
                    ClearNewServicesForm();

                }, "Success", "New service added and selected.");
            }
        }
    }
    #endregion

    #region Manage Current Services

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
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetailsByServiceID(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        //CheckStatusForButtonVisibility();

        //scroll page to bottom
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.onload=function(){window.scrollTo(0,document.body.scrollHeight)};", true);
    }

    protected void CheckStatusForButtonVisibility()
    {
        foreach (var item in ServiceDetailsListView.Items)
        {
            switch ((item.FindControl("StatusLabel") as Label).Text.Trim().ToLower())
            {
                case null:
                    (item.FindControl("StartServiceButton") as LinkButton).Visible = true;
                    (item.FindControl("FinishServiceButton") as LinkButton).Visible = false;
                    (item.FindControl("RemoveServiceDetailButton") as LinkButton).Visible = true;
                    break;
                case "started":
                    (item.FindControl("StartServiceButton") as LinkButton).Visible = false;
                    (item.FindControl("FinishServiceButton") as LinkButton).Visible = true;
                    (item.FindControl("RemoveServiceDetailButton") as LinkButton).Visible = false;
                    break;
                case "done":
                    (item.FindControl("StartServiceButton") as LinkButton).Visible = false;
                    (item.FindControl("FinishServiceButton") as LinkButton).Visible = false;
                    (item.FindControl("RemoveServiceDetailButton") as LinkButton).Visible = false;
                    break;
                default:
                    (item.FindControl("StartServiceButton") as LinkButton).Visible = true;
                    (item.FindControl("FinishServiceButton") as LinkButton).Visible = false;
                    (item.FindControl("RemoveServiceDetailButton") as LinkButton).Visible = true;
                    break;
            }
        }
    }

    protected void DeselectServiceButton_Click(object sender, EventArgs e)
    {
        //deselect currently selected item
        ServicesListView.SelectedIndex = -1;

        ResetServiceDetailsListView();
    }
    #endregion

    protected void ServiceDetailsListView_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        ResetServiceDetailsListView();
        ServiceDetailsListView.SelectedIndex = e.NewSelectedIndex;
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;

        ListViewItem serviceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
        int serviceId = int.Parse((serviceRow.FindControl("ServiceIDLabel") as Label).Text);
        ServiceController sysmgr = new ServiceController();
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetailsByServiceID(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ListViewItem selectedServiceDetail = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int selectedServiceDetailID = int.Parse((selectedServiceDetail.FindControl("ServiceDetailIDLabel") as Label).Text.Trim());
        string selectedServiceDetailDescription = (selectedServiceDetail.FindControl("DescriptionLabel") as Label).Text.Trim();

        List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_CurrentServiceDetailPartsByServiceDetailID(selectedServiceDetailID);

        SelectedServiceDetailDescriptionLabel.Text = selectedServiceDetailDescription;

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
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetailsByServiceID(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.DataBind();

        ListViewItem serviceDetailRow = ServiceDetailsListView.EditItem;

        var commentsTB = serviceDetailRow.FindControl("ServiceDetailCommentsTextArea");
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
        List<ServiceDetailPOCO> serviceDetailList = sysmgr.List_ServiceDetailsByServiceID(serviceId);
        ServiceDetailsListView.DataSource = serviceDetailList;
        ServiceDetailsListView.SelectedIndex = -1;
        ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
        ServiceDetailsListView.DataBind();

        //CheckStatusForButtonVisibility();

        ServiceDetailPartsListView.DataSource = null;
        ServiceDetailPartsListView.DataBind();
    }

    protected void ServiceDetailsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int i = e.Item.DisplayIndex;

        if ((e.CommandName.Equals("Edit")) ||(e.CommandName.Equals("Select")))
        {
            ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)0;
            ServiceDetailsPanel.Visible = true;
        }
        else if (e.CommandName.Equals("Cancel"))
        {
            ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
            ServiceDetailsPanel.Visible = true;
        }
        else if (e.CommandName.Equals("Update"))
        {
            ServiceDetailsListView.InsertItemPosition = (InsertItemPosition)2;
            ServiceDetailsPanel.Visible = true;

            ListViewItem editRow = ServiceDetailsListView.EditItem;

            int serviceId = int.Parse((editRow.FindControl("ServiceIDLabel") as Label).Text);
            string existingComments = (editRow.FindControl("ServiceDetailCommentsLabel") as Label).Text.Trim();
            HtmlTextArea commentsTextArea = editRow.FindControl("ServiceDetailCommentsTextArea") as HtmlTextArea;

            string additionalComments = null;
            //check if there's no exisiting comment
            if (string.IsNullOrWhiteSpace(existingComments))
            {
                additionalComments += commentsTextArea.InnerText.Trim();
            }
            else
            {
                additionalComments = "; " + commentsTextArea.InnerText.Trim();
            }

            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                JobDetail updatedItem = new JobDetail();
                updatedItem.JobDetailID = int.Parse((editRow.FindControl("ServiceDetailIDLabel") as Label).Text);
                updatedItem.JobID = serviceId;
                updatedItem.Description = (editRow.FindControl("ServiceDetailDescriptionLabel") as Label).Text;
                updatedItem.JobHours = decimal.Parse((editRow.FindControl("ServiceDetailHoursLabel") as Label).Text);
                updatedItem.Comments = (editRow.FindControl("ServiceDetailCommentsLabel") as Label).Text + additionalComments;

                ServiceController sysmgr = new ServiceController();
                sysmgr.Update_ServiceDetail(updatedItem);

                //refresh current job detail list
                List<ServiceDetailPOCO> detailResults = sysmgr.List_ServiceDetailsByServiceID(serviceId);

                ServiceDetailsListView.DataSource = detailResults;
                ServiceDetailsListView.EditIndex = -1;
                ServiceDetailsListView.DataBind();
            }, "Comments Successfully Added", "Additional comments have been added to the currently selected job service.");
        }
        else if (e.CommandName.Equals("Delete"))
        {
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

                    ClearNewServicesForm();

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

                    ClearNewServicesForm();

                    //refresh current service details list
                    ServiceDetailsListView.DataSource = sysmgr.List_ServiceDetailsByServiceID(serviceID);
                    ServiceDetailsListView.DataBind();

                    //keep showing service details panel
                    ServiceDetailsPanel.Visible = true;


                }, "Service Detail Removed", "Service detail has been removed from service #" + serviceID + ".");
            }
        }
    }

    protected void ServiceDetailsListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        //nothing so far
    }

    protected void ServiceDetailPartsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
    }

    protected void AddServiceDetailPartButton_Click(object sender, EventArgs e)
    {
        //get service detail ID from currently-selected item
        ListViewItem selectedServiceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
        int jobDetailID = int.Parse((selectedServiceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);

        //fetch input in the CurrentServiceDetailPartsListView insert row
        ListViewItem serviceDetailPartInsertRow = ServiceDetailPartsListView.InsertItem;
        int insertPartID = int.Parse((serviceDetailPartInsertRow.FindControl("PartIDTextBox") as TextBox).Text);
        short insertQuantity = short.Parse((serviceDetailPartInsertRow.FindControl("QuantityTextBox") as TextBox).Text);

        MessageUserControl.TryRun(() =>
        {
            ServiceController sysmgr = new ServiceController();
            sysmgr.Add_NewServiceDetailPart(jobDetailID, insertPartID, insertQuantity);

            //refresh ListView
            ListViewItem serviceDetailRow = ServiceDetailsListView.Items[ServiceDetailsListView.SelectedIndex];
            int serviceDetailId = int.Parse((serviceDetailRow.FindControl("ServiceDetailIDLabel") as Label).Text);
            List<ServiceDetailPartPOCO> serviceDetailPartsList = sysmgr.List_CurrentServiceDetailPartsByServiceDetailID(serviceDetailId);
            ServiceDetailPartsListView.DataSource = serviceDetailPartsList;
            ServiceDetailPartsListView.DataBind();

            ResetServiceDetailsListView();

        }, "Service Detail Part added", "Part #" + insertPartID + " has been added to the selected service detail");

        //CreateNewServicePanel.Visible = false;
        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;
        MessageUserControl.Visible = true;
    }

    protected void ServiceDetailPartsListView_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        ListViewItem serviceDetailPartInsertRow = ServiceDetailPartsListView.InsertItem;
        (serviceDetailPartInsertRow.FindControl("PartIDTextBox") as TextBox).Text = null;
        (serviceDetailPartInsertRow.FindControl("QuantityTextBox") as TextBox).Text = null;

        ServiceDetailsPanel.Visible = true;
        ServiceDetailPartsPanel.Visible = true;
    }

    protected void ServiceDetailPartsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Delete"))
        {
            int i = e.Item.DisplayIndex;
            ListViewItem serviceDetailPartRow = ServiceDetailPartsListView.Items[i];
            int serviceDetailPartID = int.Parse((serviceDetailPartRow.FindControl("ServiceDetailPartIDLabel") as Label).Text);

            MessageUserControl.TryRun(() =>
            {
                ServiceController sysmgr = new ServiceController();
                sysmgr.Delete_ServiceDetailPart(serviceDetailPartID);

                ServiceDetailPartsListView.DataBind();

                ServiceDetailsPanel.Visible = true;
                ServiceDetailPartsPanel.Visible = true;
                MessageUserControl.Visible = true;
            }, "Service Detail Part Removed", "Service detail part has been removed from current service detail.");
        }
    }

    protected void ServiceDetailPartsListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {

    }

    protected void ServiceDetailListViewAddServiceDetailButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.TryRun(() =>
        {
            //fetch Service ID from selected service row
            ListViewItem selectedServiceRow = ServicesListView.Items[ServicesListView.SelectedIndex];
            int selectedServiceID = int.Parse((selectedServiceRow.FindControl("ServiceIDLabel") as Label).Text);

            //fetch entered data in the insert row
            ListViewItem insertServiceDetailRow = ServiceDetailsListView.InsertItem;
            string description = (insertServiceDetailRow.FindControl("InsertRowServiceDetailDescriptionTextBox") as TextBox).Text.Trim();
            decimal hours = decimal.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailHoursTextBox") as TextBox).Text.Trim());
            int couponId = int.Parse((insertServiceDetailRow.FindControl("InsertRowServiceDetailCouponDropDownList") as DropDownList).SelectedValue.Trim());

            System.Web.UI.HtmlControls.HtmlTextArea commentsTextArea = (System.Web.UI.HtmlControls.HtmlTextArea)insertServiceDetailRow.FindControl("ServiceDetailCommentsTextArea");
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
            sysmgr.Add_NewServiceDetail(newServiceDetail);
            //refresh Listview
            ServicesListView.DataBind();

            //select the newly added service by selecting the last record in the ListView
            ServicesListView.SelectedIndex = ServicesListView.Items.Count - 1;
            ServicesListView_SelectedIndexChanged(sender, e);

            //clear form fields
            ClearNewServicesForm();

        }, "Success", "New service added and selected.");
    }

    protected void ServiceDetailsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {

    }

    protected void ServiceDetailPartsListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {

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
    }

    protected void ServiceDetailsListView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        
    }
}