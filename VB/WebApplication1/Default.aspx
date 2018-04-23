<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.2, Version=15.2.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.2.Core, Version=15.2.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script lang="cs">
        function OnClientPopupMenuShowing(s, e) {
            // 1st approach
            for (menuItemId in e.item.items) {
                if (e.item.items[menuItemId].name == "PasteAppointment") {
                    e.item.items[menuItemId].SetEnabled(document.getElementById("hdCopiedAppointmentID").value != '');
                }
            }
        }

        function OnClientItemClick(s, e) {
            if (e.item.name.indexOf("CopyAppointment") != -1) {
                var selectedIds = ClientScheduler.GetSelectedAppointmentIds();
                if (selectedIds.length > 0) {
                    document.getElementById("hdCopiedAppointmentID").value = selectedIds[0];
                }
            }
            else if (e.item.name.indexOf("PasteAppointment") != -1) {
                ClientScheduler.PerformCallback(e.item.name);
            }
            else {
                if (s.cpMenuName == "AppointmentMenu") {
                    ASPx.SchedulerOnAptMenuClick(s, e);
                }
                else {
                    ASPx.SchedulerOnViewMenuClick(s, e);
                }
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
                ClientIDMode="AutoID" Start="2013-10-30" GroupType="Date" OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing" OnCustomCallback="ASPxScheduler1_CustomCallback"
                ResourceDataSourceID="ObjectDataSourceResources" ClientInstanceName="ClientScheduler">
                <ClientSideEvents EndCallback ="OnClientEndCallback" />
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