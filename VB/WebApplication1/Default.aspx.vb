Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.XtraScheduler

Namespace WebApplication1
    Partial Public Class [Default]
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

        End Sub

        Protected Sub ObjectDataSourceResources_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
            If Session("CustomResourceDataSource") Is Nothing Then
                Session("CustomResourceDataSource") = New CustomResourceDataSource(GetCustomResources())
            End If
            e.ObjectInstance = Session("CustomResourceDataSource")
        End Sub

        Private Function GetCustomResources() As BindingList(Of CustomResource)
            Dim resources As New BindingList(Of CustomResource)()
            resources.Add(CreateCustomResource(1, "Max Fowler"))
            resources.Add(CreateCustomResource(2, "Nancy Drewmore"))
            resources.Add(CreateCustomResource(3, "Pak Jang"))
            Return resources
        End Function

        Private Function CreateCustomResource(ByVal res_id As Integer, ByVal caption As String) As CustomResource
            Dim cr As New CustomResource()
            cr.ResID = res_id
            cr.Name = caption
            Return cr
        End Function

        Public RandomInstance As New Random()
        Private Function CreateCustomAppointment(ByVal subject As String, ByVal resourceId As Object, ByVal status As Integer, ByVal label As Integer) As CustomAppointment
            Dim apt As New CustomAppointment()
            apt.Subject = subject
            apt.OwnerId = resourceId
            Dim rnd As Random = RandomInstance
            Dim rangeInMinutes As Integer = 60 * 24
            apt.StartTime = Date.Today + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes))
            apt.EndTime = apt.StartTime.Add(TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes \ 4)))
            apt.Status = status
            apt.Label = label
            Return apt
        End Function

        Protected Sub ObjectDataSourceAppointment_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
            If Session("CustomAppointmentDataSource") Is Nothing Then
                Session("CustomAppointmentDataSource") = New CustomAppointmentDataSource(GetCustomAppointments())
            End If
            e.ObjectInstance = Session("CustomAppointmentDataSource")
        End Sub

        Private Function GetCustomAppointments() As BindingList(Of CustomAppointment)
            Dim appointments As New BindingList(Of CustomAppointment)()

            Dim resources As CustomResourceDataSource = TryCast(Session("CustomResourceDataSource"), CustomResourceDataSource)
            If resources IsNot Nothing Then
                For Each item As CustomResource In resources.Resources
                    Dim subjPrefix As String = item.Name & "'s "
                    appointments.Add(CreateCustomAppointment(subjPrefix & "meeting", item.ResID, 2, 5))
                    appointments.Add(CreateCustomAppointment(subjPrefix & "travel", item.ResID, 3, 6))
                    appointments.Add(CreateCustomAppointment(subjPrefix & "phone call", item.ResID, 0, 10))
                Next item
            End If
            Return appointments
        End Function

        Protected Sub ASPxScheduler1_PopupMenuShowing(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs)
            e.Menu.ClientInstanceName = "AppointmentPopupMenu"
            e.Menu.ClientSideEvents.PopUp = "OnClientPopupMenuShowing"
            e.Menu.ClientSideEvents.ItemClick = "OnClientItemClick"

            If e.Menu.MenuId = DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu Then
                Dim newItemCopy As New DevExpress.Web.MenuItem()
                newItemCopy.Name = "CopyAppointment"
                newItemCopy.Text = "Copy"
                newItemCopy.ItemStyle.Font.Bold = True
                e.Menu.Items.Add(newItemCopy)
                e.Menu.JSProperties("cpMenuName") = "AppointmentMenu"
            End If
            If e.Menu.MenuId = DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu Then
                Dim newItemPaste As New DevExpress.Web.MenuItem()
                newItemPaste.Name = "PasteAppointment"
                newItemPaste.Text = "Paste"
                newItemPaste.ItemStyle.Font.Bold = True
                e.Menu.Items.Insert(0, newItemPaste)
                e.Menu.JSProperties("cpMenuName") = "DefaultMenu"
            End If
        End Sub

        Protected Sub ASPxScheduler1_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.CallbackEventArgsBase)
            If e.Parameter = "PasteAppointment" AndAlso hdCopiedAppointmentID.Value <> "" Then
                Dim copyAppointmentID As Integer = Convert.ToInt32(hdCopiedAppointmentID.Value)
                Dim sourceAppointment As Appointment = ASPxScheduler1.Storage.Appointments.GetAppointmentById(copyAppointmentID)
                If sourceAppointment IsNot Nothing Then
                    Dim newAppointment As Appointment = ASPxScheduler1.Storage.CreateAppointment(sourceAppointment.Type)
                    newAppointment.Description = sourceAppointment.Description
                    newAppointment.LabelKey = sourceAppointment.LabelKey
                    newAppointment.Location = sourceAppointment.Location
                    newAppointment.ResourceId = ASPxScheduler1.SelectedResource.Id

                    newAppointment.Subject = sourceAppointment.Subject
                    newAppointment.StatusKey = sourceAppointment.StatusKey

                    newAppointment.Start = ASPxScheduler1.SelectedInterval.Start
                    newAppointment.End = newAppointment.Start + sourceAppointment.Duration

                    ASPxScheduler1.Storage.Appointments.Add(newAppointment)
                    ASPxScheduler1.JSProperties("cpCallBackParameter") = "PasteAppointment"
                End If
            End If
        End Sub
    End Class
End Namespace