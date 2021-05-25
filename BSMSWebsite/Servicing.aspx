﻿<%@ Page Title="Servicing" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Servicing.aspx.cs" Inherits="Servicing" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/User_Controls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="ServicingContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-4 mt-5">Servicing</h1>
            <p class="lead">Bike In-Store Servicing Management</p>

            <%--<div>
                <asp:Button ID="ManageCurrentServicesButton" Text="Manage Current Services" runat="server" CssClass="btn btn-primary" OnClick="ManageCurrentServicesButton_Click"/>
                <asp:Button ID="CreateNewServiceButton" Text="Add New Service" runat="server" CssClass="btn btn-success" OnClick="CreateNewServiceButton_Click"/>
            </div>--%>
        </div>
    </div>

    <div class="container">
        <div class="d-flex align-items-center justify-content-between">
            <h2 class="card-title">Current Services</h2>

            <button type="button" class="btn btn-success" data-toggle="modal" data-target="#addNewServiceModal">
                Add New Service
            </button>
        </div>
        <p class="card-subtitle mb-2 text-muted">Select a service to view or manage.</p>

        <%-- Add New Service Modal --%>
        <div class="modal fade" id="addNewServiceModal" tabindex="-1" aria-labelledby="#addNewServiceModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
                <div class="modal-content">
                    <div class="modal-header">
                        <h2 class="modal-title" id="addNewServiceModalLabel">Add New Service</h2>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true"><i class="bi bi-x"></i></span>
                        </button>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <p class="text-muted">Add a new service by recording the required information.</p>
                            </div>                            

                            <div class="row text-left">
                                <div class="form-group col-md-6">
                                    <asp:Label runat="server" Text="Customer" AssociatedControlID="CustomerDropDownList"/>
                                    <asp:ObjectDataSource runat="server" ID="CustomerODS"
                                        OldValuesParameterFormatString="original_{0}"
                                        SelectMethod="List_CustomersByLastName"
                                        TypeName="BSMSSystem.BLL.CustomerController" />
                                    <asp:DropDownList ID="CustomerDropDownList" runat="server"
                                        DataSourceID="CustomerODS"
                                        DataTextField="Name"
                                        DataValueField="CustomerID"
                                        AppendDataBoundItems="true"
                                        CssClass="custom-select"
                                        ToolTip="Select the customer for the service.">
                                        <asp:ListItem Text="Select..." Value="0" Selected="True"/>
                                    </asp:DropDownList>
                                </div>
                    
                                <div class="form-group col-md-6">
                                    <asp:Label runat="server" AssociatedControlID="ServiceVehicleIdentificationTextBox" Text="Vehicle Identification"/>
                                    <asp:TextBox runat="server" ID="ServiceVehicleIdentificationTextBox" CssClass="form-control"/>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card h-100">
                                        <div class="card-body">
                                            <h4 class="card-title">Add Service Detail</h4>
                                            <p class="card-subtitle mb-2 text-muted">Add a service detail.</p>

                                            <div class="form-group">
                                                <asp:Label Text="Description" runat="server" AssociatedControlID="NewServiceModalServiceDetailDescriptionTextBox"/>
                                                <asp:TextBox runat="server" ID="NewServiceModalServiceDetailDescriptionTextBox" CssClass="form-control"/>
                                            </div>
                                            <div class="form-group row">
                                                <div class="col-md-5">
                                                    <asp:Label Text="Hours" runat="server" AssociatedControlID="NewServiceModalServiceDetailHoursTextBox"/>
                                                    <asp:TextBox runat="server" ID="NewServiceModalServiceDetailHoursTextBox" CssClass="form-control"/>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:Label Text="Coupon" runat="server" AssociatedControlID="NewServiceModalCouponDropDownList"/>
                                                    <asp:ObjectDataSource runat="server" ID="CouponODS"
                                                        OldValuesParameterFormatString="original_{0}"
                                                        SelectMethod="List_Coupons"
                                                        TypeName="BSMSSystem.BLL.ServiceController" />
                                                    <asp:DropDownList runat="server" CssClass="custom-select" ID="NewServiceModalCouponDropDownList"
                                                        DataSourceID="CouponODS"
                                                        DataTextField="CouponIDValue"
                                                        DataValueField="CouponID"
                                                        AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0" Selected="True">Select...</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label Text="Comments" runat="server" AssociatedControlID="NewServiceModalServiceDetailCommentsTextBox"/>
                                                <textarea  id="NewServiceModalServiceDetailCommentsTextBox" runat="server" rows="2" class="form-control" style="min-height:4rem;"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button Text="Add New Service" runat="server" ID="NewServiceModalAddNewServiceButton" CssClass="btn btn-success" OnClick="AddNewServiceButton_Click"/>
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <asp:ObjectDataSource ID="ServicesODS" runat="server"
                OldValuesParameterFormatString="original_{0}"
                SelectMethod="List_CurrentServices"
                TypeName="BSMSSystem.BLL.ServiceController" />

                <asp:ListView ID="ServicesListView" runat="server"
                    DataSourceID="ServicesODS"
                    DataKeyNames="ServiceID"
                    OnSelectedIndexChanged="ServicesListView_SelectedIndexChanged"
                    OnItemDataBound="ServicesListView_ItemDataBound">
                    <EmptyDataTemplate>
                        <div class="table-responsive">
                            <table runat="server" class="table table-striped table-borderless mb-0">
                                <caption>List of current services</caption>
                                <thead>
                                    <tr class="thead-dark text-center align-middle">
                                        <th scope="col">No data was returned.</th>
                                    </tr>
                                <thead>
                            </table>
                        </div>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table-responsive">
                            <table runat="server" id="itemPlaceholderContainer" class="table table-hover table-striped table-borderless mb-0">
                                <caption>List of current services</caption>
                                <thead>
                                    <tr runat="server" class="thead-dark text-center align-middle">
                                        <th runat="server" scope="col">#</th>
                                        <th runat="server" scope="col">In</th>
                                        <th runat="server" scope="col">Started</th>
                                        <th runat="server" scope="col">Done</th>
                                        <th runat="server" scope="col">Customer</th>
                                        <th runat="server" scope="col">Phone</th>
                                        <th runat="server" scope="col"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </tbody>
                            </table>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="text-center align-middle">
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("ServiceID") %>' runat="server" ID="ServiceIDLabel" />
                            </td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("In","{0:MMM dd, yyyy}") %>' runat="server" ID="InLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("Started","{0:MMM dd, yyyy}") %>' runat="server" ID="StartedLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("Done","{0:MMM dd, yyyy}") %>' runat="server" ID="DoneLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("CustomerName") %>' runat="server" ID="CustomerNameLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("ContactNumber") %>' runat="server" ID="ContactNumberLabel" />
                                <asp:Label Text='<%# Eval("VehicleIdentification") %>' runat="server" ID="VehicleIdentificationLabel" Visible="false" />
                            </td>
                            <td class="align-middle p-1">
                                <asp:LinkButton runat="server" CommandName="Select" Text="Select" ID="SelectButton" CssClass="btn btn-link btn-block text-decoration-none p-0">
                                    <i class="bi bi-gear-fill"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <SelectedItemTemplate>
                        <tr class="text-center table-active shadow-sm">
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("ServiceID") %>' runat="server" ID="ServiceIDLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("In","{0:MMM dd, yyyy}") %>' runat="server" ID="InLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("Started","{0:MMM dd, yyyy}") %>' runat="server" ID="StartedLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("Done","{0:MMM dd, yyyy}") %>' runat="server" ID="DoneLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("CustomerName") %>' runat="server" ID="CustomerNameLabel" /></td>
                            <td class="align-middle">
                                <asp:Label Text='<%# Eval("ContactNumber") %>' runat="server" ID="ContactNumberLabel" />
                                <asp:Label Text='<%# Eval("VehicleIdentification") %>' runat="server" ID="VehicleIdentificationLabel" Visible="false" /></td>
                            <td class="align-middle p-1">
                                <asp:LinkButton Text="Deselect" runat="server" ID="DeselectButton" OnClick="DeselectServiceButton_Click" CssClass="btn btn-link btn-block text-decoration-none p-0">
                                    <i class="bi bi-x h4"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </SelectedItemTemplate>
                </asp:ListView>
            </div>
        </div>

        <div class="row">
            <div class="container-fluid pb-3">
                <asp:Panel runat="server" ID="ServiceDetailsPanel" CssClass="card">
                    <div class="card-body">
                        <div class="row mb-4">
                            <div class="col-12">
                                <h3 class="card-title h4">Service #<asp:Label ID="SelectedServiceIDLabel" runat="server"/> Details</h3>
                                <p class="card-subtitle text-muted">Manage a service detail of <asp:Label ID="SelectedServiceCustomerNameLabel" runat="server" CssClass="font-weight-bold"/>'s vehicle, <asp:Label ID="SelectedServiceVehicleIdentificationLabel" runat="server" CssClass="font-weight-bold"/>, to manage.</p>
                            </div>
                        </div>                        

                        <div class="row">
                            <div class="col-12">
                                <asp:ListView runat="server" ID="ServiceDetailsListView"
                                    DataKeyNames="ServiceDetailID"
                                    OnSelectedIndexChanging="ServiceDetailsListView_SelectedIndexChanging"
                                    OnItemCommand="ServiceDetailsListView_ItemCommand"
                                    OnItemEditing="ServiceDetailsListView_ItemEditing"
                                    OnItemUpdating="ServiceDetailsListView_ItemUpdating"
                                    OnItemCanceling="ServiceDetailsListView_ItemCanceling"
                                    OnItemInserting="ServiceDetailsListView_ItemInserting"
                                    OnItemDeleting="ServiceDetailsListView_ItemDeleting"
                                    OnItemDataBound="ServiceDetailsListView_ItemDataBound"
                                    InsertItemPosition="LastItem">
                                    <LayoutTemplate>
                                        <div class="table-responsive">
                                            <table runat="server" id="itemPlaceholderContainer" class="table table-striped table-hover table-borderless mb-0">
                                                <caption>List of current service details</caption>
                                                <thead>
                                                    <tr runat="server" class="thead-dark text-center align-middle">
                                                        <th runat="server" scope="col">Description</th>
                                                        <th runat="server" scope="col">Hours</th>
                                                        <th runat="server" scope="col">Coupon</th>
                                                        <th runat="server" scope="col">Comments</th>
                                                        <th runat="server" scope="col">Status</th>
                                                        <th runat="server" scope="col"></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr runat="server" id="itemPlaceholder"></tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <div class="table-responsive">
                                            <table runat="server" class="table table-striped table-hover table-sm">
                                                <caption>List of current service details</caption>
                                                <thead>
                                                    <tr class="table-info text-center align-middle">
                                                        <th scope="col" class="h-100">No service details found.</th>
                                                    </tr>
                                                <thead>
                                            </table>
                                        </div>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr class="text-center">
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Eval("ServiceID") %>' runat="server" ID="ServiceIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("ServiceDetailHours") %>' runat="server" ID="ServiceDetailHoursLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("Coupon") %>' runat="server" ID="CouponLabel" /></td>
                                            <td class="align-middle">
                                                <asp:LinkButton runat="server" ID="EditServiceDetailButton" CommandName="Edit" 
                                                            CssClass="btn btn-link btn-block text-decoration-none d-flex justify-content-between align-content-center" ToolTip="Add Comment">
                                                    <asp:Label Text='<%# Eval("Comments") %>' runat="server" ID="CommentsLabel" />
                                                    <i class="bi bi-pencil-fill"></i>
                                                </asp:LinkButton></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("Status") %>' runat="server" ID="StatusLabel" /></td>
                                            <td class="align-middle">
                                                <div class="dropdown">
                                                    <button type="button" class="btn btn-link btn-block text-decoration-none" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="serviceDetailsActionMenuButton" data-boundary="viewport">
                                                        <i class="bi bi-three-dots-vertical"></i>
                                                    </button>
                                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="serviceDetailsActionMenuButton">
                                                        <asp:Button runat="server" ID="ManagePartsButton" CommandName="Select"
                                                            CssClass="dropdown-item"
                                                            Text="Select" ToolTip="Select Service Detail" type="button"/>
                                                        <asp:Button runat="server" ID="ServiceStatusButton" CommandName="StatusUp"
                                                            Text="Start" CssClass="dropdown-item" ToolTip="Start Service Detail" type="button"/>
                                                        <asp:Button runat="server" ID="RemoveServiceDetailButton" CommandName="Delete"
                                                            CssClass="dropdown-item"
                                                            Text="Remove" ToolTip="Remove Service Detail" type="button"/>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <SelectedItemTemplate>
                                        <tr class="table-active text-center">
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Eval("ServiceID") %>' runat="server" ID="ServiceIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("ServiceDetailHours") %>' runat="server" ID="ServiceDetailHoursLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("Coupon") %>' runat="server" ID="CouponLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("Comments") %>' runat="server" ID="CommentsLabel" /></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Eval("Status") %>' runat="server" ID="StatusLabel" />
                                            </td>
                                            <td class="align-middle">
                                                <asp:LinkButton runat="server" ID="DeselectButton" OnClick="DeselectServiceDetailButton_Click" CssClass="btn btn-link btn-block text-decoration-none">
                                                    <i class="bi bi-x h4"></i>
                                                </asp:LinkButton></td>
                                        </tr>
                                    </SelectedItemTemplate>
                                    <EditItemTemplate>
                                        <tr class="table-active text-center">
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Bind("ServiceID") %>' runat="server" ID="ServiceIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Bind("Description") %>' runat="server" ID="ServiceDetailDescriptionLabel"/></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("ServiceDetailHours") %>' runat="server" ID="ServiceDetailHoursLabel"/></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("Coupon") %>' runat="server" ID="CouponLabel"/></td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("Comments") %>' runat="server" ID="ServiceDetailCommentsLabel" CssClass="d-block mb-2"/>
                                                <div class="d-block d-flex align-items-center">
                                                    <textarea runat="server" ID="ServiceDetailCommentsTextArea" class="form-control flex-grow-1"
                                                        ToolTip="Add comments" Placeholder="Add comments..." style="min-height:2.5rem; min-width:12rem;" rows="1"/>
                                                    <asp:LinkButton ID="UpdateServiceDetailButton" runat="server" CommandName="Update" CssClass="btn btn-link text-decoration-none text-success py-0 px-2 mx-auto" ToolTip="Add Comment">
                                                        <i class="bi bi-arrow-up-circle-fill h4"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="ClearServiceDetailButton" runat="server" CommandName="Cancel" CssClass="btn btn-link text-decoration-none text-secondary py-0 px-2" ToolTip="Cancel Comment">
                                                        <i class="bi bi-x-circle-fill h4"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("Status") %>' runat="server" ID="StatusLabel" /></td>
                                            <td></td>
                                        </tr>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <tr class="table-success text-center">
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" Visible="false"/>
                                                <asp:Label Text='<%# Bind("ServiceID") %>' runat="server" ID="ServiceIDLabel" Visible="false"/>
                                                <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="InsertRowServiceDetailDescriptionTextBox" CssClass="form-control"/></td>
                                            <td class="align-middle">
                                                <asp:TextBox Text='<%# Bind("ServiceDetailHours") %>' runat="server" ID="InsertRowServiceDetailHoursTextBox" CssClass="form-control"/></td>
                                            <td class="align-middle">
                                                <asp:DropDownList runat="server" CssClass="custom-select" ID="InsertRowServiceDetailCouponDropDownList"
                                                    DataSourceID="CouponODS"
                                                    DataTextField="CouponIDValue"
                                                    DataValueField="CouponID"
                                                    AppendDataBoundItems="true">
                                                    <asp:ListItem Value="0" Selected="True">Select...</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td class="align-middle">
                                                <%--<asp:TextBox Text='<%# Bind("Comments") %>' runat="server" ID="InsertRowServiceDetailCommentsTextBox" CssClass="form-control text-wrap" style="max-width:16rem;"/>--%>
                                                <textarea runat="server" ID="ServiceDetailCommentsTextArea" class="form-control" style="min-height:2.5rem;" rows="1"/>
                                            </td>
                                            <td class="align-middle">
                                                <asp:Label Text='<%# Bind("Status") %>' runat="server" ID="StatusLabel" /></td>
                                            <td class="align-middle">
                                                <asp:LinkButton ID="ServiceDetailListViewAddServiceDetailButton" runat="server" CommandName="Insert" Text="Add Service Detail" CssClass="btn btn-link text-decoration-none text-success p-0" ToolTip="Add Service Detail" OnClick="ServiceDetailListViewAddServiceDetailButton_Click">
                                                    <i class="bi bi-plus-circle-fill h4"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="ServiceDetailListViewClearServiceDetailButton" runat="server" CommandName="Cancel" Text="Clear" CssClass="btn btn-link text-decoration-none text-secondary p-0" ToolTip="Clear">
                                                    <i class="bi bi-x-circle-fill h4"></i>
                                                </asp:LinkButton></td>
                                        </tr>
                                    </InsertItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>

                        <div class="row">
                            <div class="container-fluid pb-3">
                                <asp:Panel runat="server" ID="ServiceDetailPartsPanel" CssClass="card">
                                    <div class="card-body">
                                        <div class="row mb-4">
                                            <div class="col-12">
                                                <h4 class="card-title"><asp:Label ID="SelectedServiceDetailDescriptionLabel" runat="server"/> Service Detail Parts</h4>
                                                <p class="card-subtitle mb-2 text-muted">Add, remove, update, or view service detail parts.</p>
                                    
                                                <asp:ListView runat="server" ID="ServiceDetailPartsListView"
                                                    DataKeyNames="ServiceDetailID"
                                                    InsertItemPosition="LastItem"
                                                    OnItemCommand="ServiceDetailPartsListView_ItemCommand"
                                                    OnItemEditing="ServiceDetailPartsListView_ItemEditing"
                                                    OnItemInserting="ServiceDetailPartsListView_ItemInserting"
                                                    OnItemCanceling="ServiceDetailPartsListView_ItemCanceling"
                                                    OnItemDeleting="ServiceDetailPartsListView_ItemDeleting">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive">
                                                            <table runat="server" id="itemPlaceholderContainer" class="table table-striped table-hover table-sm mb-0">
                                                                <caption>List of current service detail parts</caption>
                                                                <thead>
                                                                    <tr runat="server" class="thead-dark text-center align-middle">
                                                                        <th runat="server" scope="col" hidden>Service Detail ID</th>
                                                                        <th runat="server" scope="col" hidden>Service Detail Part ID</th>
                                                                        <th runat="server" scope="col">Part #</th>
                                                                        <th runat="server" scope="col">Description</th>
                                                                        <th runat="server" scope="col">Quantity</th>
                                                                        <th runat="server" scope="col">Actions</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr runat="server" id="itemPlaceholder"></tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </LayoutTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive">
                                                            <table runat="server" class="table table-striped table-hover table-sm">
                                                                <caption>List of current services</caption>
                                                                <thead>
                                                                    <tr class="table-info text-center align-middle">
                                                                        <th scope="col">No current service detail parts on record.</th>
                                                                    </tr>
                                                                <thead>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                    <ItemTemplate>
                                                        <tr class="text-center">
                                                            <td hidden class="align-middle">
                                                                <asp:Label Text='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" /></td>
                                                            <td hidden class="align-middle">
                                                                <asp:Label Text='<%# Eval("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDLabel" /></td>
                                                            <td class="align-middle" style="min-width:4rem;">
                                                                <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:Label Text='<%# Eval("PartDescription") %>' runat="server" ID="PartDescriptionLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                                                            <td class="align-middle">
                                                                <span class="btn-group btn-group-sm" role="group">
                                                                    <asp:LinkButton runat="server" CommandName="Edit" Text="Edit" ID="EditPartButton" CssClass="btn btn-primary btn-sm" ToolTip="Edit Quantity"><i class="far fa-edit"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="RemovePartButton" CssClass="btn btn-danger btn-sm" ToolTip="Remove Part(s)"><i class="far fa-trash-alt"></i></asp:LinkButton>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <SelectedItemTemplate>
                                                        <tr class="table-warning text-center">
                                                            <td hidden>
                                                                <asp:Label Text='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" /></td>
                                                            <td hidden>
                                                                <asp:Label Text='<%# Eval("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDLabel" /></td>
                                                            <td>
                                                                <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                                                            <td>
                                                                <asp:Label Text='<%# Eval("PartDescription") %>' runat="server" ID="PartDescriptionLabel" /></td>
                                                            <td>
                                                                <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                                                            <td>
                                                                <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelEditPartButton" CssClass="btn btn-secondary btn-sm"/></td>
                                                        </tr>
                                                    </SelectedItemTemplate>
                                                    <EditItemTemplate>
                                                        <tr class="table-warning text-center">
                                                            <td hidden>
                                                                <asp:Label Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" /></td>
                                                            <td hidden>
                                                                <asp:Label Text='<%# Bind("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:TextBox Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" CssClass="form-control"/></td>
                                                                <%--<asp:ObjectDataSource ID="PartsListODS" runat="server"
                                                                    OldValuesParameterFormatString="original_{0}"
                                                                    SelectMethod="List_Parts"
                                                                    TypeName="BSMS.System.BLL.ServiceController"/>
                                                                <asp:DropDownList ID="PartsDropDownList" runat="server"
                                                                    CssClass="custom-select"
                                                                    DataSourceID="PartsListODS"
                                                                    DataTextField="Description"
                                                                    DataValueField="PartID"
                                                                    AppendDataBoundItems="true">
                                                                    <asp:ListItem Value="0" Selected="True">Select...</asp:ListItem>
                                                                </asp:DropDownList></td>--%>
                                                            <td>
                                                                <asp:Label Text='<%# Bind("PartDescription") %>' runat="server" ID="PartDescriptionLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" CssClass="form-control"/></td>
                                                            <td class="align-middle">
                                                                <span class="btn-group btn-group-sm" role="group">
                                                                    <asp:LinkButton ID="AddServiceDetailPartButton" runat="server" CommandName="Insert" Text="Add Part" CssClass="btn btn-success btn-sm" OnClick="AddServiceDetailPartButton_Click" ToolTip="Add Part"><i class="fas fa-plus"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="ClearServiceDetailPartButton" runat="server" CommandName="Cancel" Text="Clear" CssClass="btn btn-secondary btn-sm" ToolTip="Clear"><i class="fas fa-eraser"></i></asp:LinkButton>
                                                                </span></td>
                                                        </tr>
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <tr class="table-success text-center">
                                                            <td hidden>
                                                                <asp:Label Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" /></td>
                                                            <td hidden>
                                                                <asp:Label Text='<%# Bind("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:TextBox Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" CssClass="form-control"/></td>
                                                                <%--<asp:ObjectDataSource ID="PartsListODS" runat="server"
                                                                    OldValuesParameterFormatString="original_{0}"
                                                                    SelectMethod="List_Parts"
                                                                    TypeName="BSMS.System.BLL.ServiceController"/>
                                                                <asp:DropDownList ID="PartsDropDownList" runat="server"
                                                                    CssClass="custom-select"
                                                                    DataSourceID="PartsListODS"
                                                                    DataTextField="Description"
                                                                    DataValueField="PartID"
                                                                    AppendDataBoundItems="true">
                                                                    <asp:ListItem Value="0" Selected="True">Select...</asp:ListItem>
                                                                </asp:DropDownList></td>--%>
                                                            <td>
                                                                <asp:Label Text='<%# Bind("PartDescription") %>' runat="server" ID="PartDescriptionLabel" /></td>
                                                            <td class="align-middle">
                                                                <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" CssClass="form-control"/></td>
                                                            <td class="align-middle">
                                                                <span class="btn-group btn-group-sm" role="group">
                                                                    <asp:LinkButton ID="AddServiceDetailPartButton" runat="server" CommandName="Insert" Text="Add Part" CssClass="btn btn-success btn-sm" OnClick="AddServiceDetailPartButton_Click" ToolTip="Add Part"><i class="fas fa-plus"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="ClearServiceDetailPartButton" runat="server" CommandName="Cancel" Text="Clear" CssClass="btn btn-secondary btn-sm" ToolTip="Clear"><i class="fas fa-eraser"></i></asp:LinkButton>
                                                                </span></td>
                                                        </tr>
                                                    </InsertItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
            
    </div>
    
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

</asp:Content>