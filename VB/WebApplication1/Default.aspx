<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v24.2, Version=24.2.1.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v24.2.Core, Version=24.2.1.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v24.2.Core.Desktop, Version=24.2.1.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script lang="cs">
        function OnClientPopupMenuShowing(s, e) {
            for (menuItemId in e.item.items) {
                if (e.item.items[menuItemId].name == "PasteAppointment") {
                    e.item.items[menuItemId].SetEnabled(document.getElementById("hdCopiedAppointmentID").value != '');
                }
            }
        }

        function onMenuItemClicked(s, e) {
            if (e.itemName.indexOf("CopyAppointment") != -1) {
                var selectedIds = ClientScheduler.GetSelectedAppointmentIds();
                if (selectedIds.length > 0) {
                    document.getElementById("hdCopiedAppointmentID").value = selectedIds[0];
                }
            }
            else if (e.itemName.indexOf("PasteAppointment") != -1) {
                ClientScheduler.PerformCallback(e.itemName);
            }
        }

        function OnClientEndCallback(s, e) {
            if (ClientScheduler.cpCallBackParameter == "PasteAppointment") {
                document.getElementById("hdCopiedAppointmentID").value = '';
            }
        }
    </script>

    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hdCopiedAppointmentID" runat="server" />
            <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" AppointmentDataSourceID="ObjectDataSourceAppointment"
                ClientIDMode="AutoID" GroupType="Date" OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing" OnCustomCallback="ASPxScheduler1_CustomCallback"
                ResourceDataSourceID="ObjectDataSourceResources" ClientInstanceName="ClientScheduler">
                <ClientSideEvents EndCallback ="OnClientEndCallback" MenuItemClicked="onMenuItemClicked" />
                <Storage>
                    <Appointments AutoRetrieveId="True">
                        <Mappings
                            AllDay="AllDay"
                            AppointmentId="Id"
                            Description="Description"
                            End="EndTime"
                            Label="Label"
                            Location="Location"
                            ReminderInfo="ReminderInfo"
                            ResourceId="OwnerId"
                            Start="StartTime"
                            Status="Status"
                            Subject="Subject"
                            Type="EventType" />
                    </Appointments>
                    <Resources>
                        <Mappings
                            Caption="Name"
                            ResourceId="ResID" />
                    </Resources>
                </Storage>

                <Views>
                    <DayView>
                        <TimeRulers>
                            <cc1:TimeRuler AlwaysShowTimeDesignator="False" ShowCurrentTime="Auto"></cc1:TimeRuler>
                        </TimeRulers>
                        <DayViewStyles ScrollAreaHeight="600px">
                        </DayViewStyles>
                    </DayView>

                    <WorkWeekView>
                        <TimeRulers>
                            <cc1:TimeRuler></cc1:TimeRuler>
                        </TimeRulers>
                    </WorkWeekView>
                </Views>
            </dxwschs:ASPxScheduler>
            <br />
            <br />
            <asp:Button ID="ButtonPostBack" runat="server" Text="Post Back" />

            <asp:ObjectDataSource ID="ObjectDataSourceResources" runat="server" OnObjectCreated="ObjectDataSourceResources_ObjectCreated" SelectMethod="SelectMethodHandler" TypeName="WebApplication1.CustomResourceDataSource"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObjectDataSourceAppointment" runat="server" DataObjectTypeName="WebApplication1.CustomAppointment" DeleteMethod="DeleteMethodHandler" InsertMethod="InsertMethodHandler" SelectMethod="SelectMethodHandler" TypeName="WebApplication1.CustomAppointmentDataSource" UpdateMethod="UpdateMethodHandler" OnObjectCreated="ObjectDataSourceAppointment_ObjectCreated"></asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
