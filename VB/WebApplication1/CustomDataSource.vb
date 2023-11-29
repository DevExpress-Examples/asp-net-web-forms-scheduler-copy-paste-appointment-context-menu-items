Imports System
Imports System.Collections
Imports System.ComponentModel

Namespace WebApplication1

    Public Class CustomAppointmentDataSource

        Private appts As BindingList(Of CustomAppointment)

        Public Sub New(ByVal parAppts As BindingList(Of CustomAppointment))
            If parAppts Is Nothing Then DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("Appointments")
            Appointments = parAppts
        End Sub

        Public Sub New()
            Me.New(New BindingList(Of CustomAppointment)())
        End Sub

        Public Property Appointments As BindingList(Of CustomAppointment)
            Get
                Return appts
            End Get

            Set(ByVal value As BindingList(Of CustomAppointment))
                If TypeOf value Is BindingList(Of CustomAppointment) AndAlso TryCast(value, BindingList(Of CustomAppointment)).Count > 0 Then
                    appts = value
                    For i As Integer = 0 To appts.Count - 1
                        appts(i).Id = i
                    Next
                End If
            End Set
        End Property

'#Region "ObjectDataSource methods                        "
        Public Function GetSourceAppointment(ByVal apptId As Object) As CustomAppointment
            For Each item As CustomAppointment In Appointments
                If Convert.ToInt32(item.Id) = Convert.ToInt32(apptId) Then
                    Return item
                End If
            Next

            Return Nothing
        End Function

        Public Function InsertMethodHandler(ByVal customAppt As CustomAppointment) As Object
            Dim lastId As Integer = 0
            For Each item As CustomAppointment In Appointments
                If Convert.ToInt32(item.Id) > lastId Then lastId = Convert.ToInt32(item.Id)
            Next

            customAppt.Id = lastId + 1
            Appointments(Appointments.Count - 1).Id = customAppt.Id
            Return customAppt.Id
        End Function

        Public Sub DeleteMethodHandler(ByVal customAppt As CustomAppointment)
            Dim currentAppt As CustomAppointment = GetSourceAppointment(customAppt.Id)
            If customAppt IsNot Nothing Then Appointments.Remove(customAppt)
        End Sub

        Public Sub UpdateMethodHandler(ByVal customAppt As CustomAppointment)
            Dim currentAppt As CustomAppointment = GetSourceAppointment(customAppt.Id)
            If customAppt IsNot Nothing Then
                currentAppt.AllDay = customAppt.AllDay
                currentAppt.Description = customAppt.Description
                currentAppt.EndTime = customAppt.EndTime
                currentAppt.EventType = customAppt.EventType
                currentAppt.Id = customAppt.Id
                currentAppt.Label = customAppt.Label
                currentAppt.Location = customAppt.Location
                currentAppt.OwnerId = customAppt.OwnerId
                currentAppt.RecurrenceInfo = customAppt.RecurrenceInfo
                currentAppt.ReminderInfo = customAppt.ReminderInfo
                currentAppt.StartTime = customAppt.StartTime
                currentAppt.Status = customAppt.Status
                currentAppt.Subject = customAppt.Subject
            End If
        End Sub

        Public Function SelectMethodHandler() As IEnumerable
            Return Appointments
        End Function
'#End Region
    End Class

    Public Class CustomResourceDataSource

        Private resourcesField As BindingList(Of CustomResource)

        Public Sub New(ByVal parResources As BindingList(Of CustomResource))
            If parResources Is Nothing Then DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("Resources")
            Resources = parResources
        End Sub

        Public Sub New()
            Me.New(New BindingList(Of CustomResource)())
        End Sub

        Public Property Resources As BindingList(Of CustomResource)
            Get
                Return resourcesField
            End Get

            Set(ByVal value As BindingList(Of CustomResource))
                resourcesField = value
            End Set
        End Property

'#Region "ObjectDataSource methods"
        Public Function SelectMethodHandler() As IEnumerable
            Return Resources
        End Function
'#End Region
    End Class
End Namespace
