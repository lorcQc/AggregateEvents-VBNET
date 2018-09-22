Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Model
    Public Class Project
        Inherits Entity
        Implements Interfaces.IAggregateRoot

        Public Property Name As String

        Private _Status As String = "New"
        Public Property Status As String
            Get
                Return _Status
            End Get
            Private Set(value As String)
                _Status = value
            End Set
        End Property 'Get / Private Set


        Private _tasks As List(Of Task) = New List(Of Task)()
        Private _activityLog As List(Of String) = New List(Of String)
        Private _hoursLimit As Integer = 10

        Public ReadOnly Property Tasks As IEnumerable(Of Task)
            Get
                Return _tasks.AsReadOnly()
            End Get
        End Property

        Public Sub New()
            AggregateEvents.Register(Of TaskCompletedEvent)(AddressOf HandleTaskCompleted)
            AggregateEvents.Register(Of TaskHoursUpdatedEvent)(AddressOf HandleTaskHoursUpdated)
        End Sub

        Private Sub HandleTaskHoursUpdated(ByVal taskHoursUpdatedEvent As TaskHoursUpdatedEvent)
            If taskHoursUpdatedEvent.Task.ProjectId <> Id Then Exit Sub

            If Not VerifyHoursWithinLimit() Then
                Log("Update would exceed project hour limit.")
                Throw New Exception("Project hour limit exceeded.")
            End If

            UpdateStatus()
        End Sub

        Private Function VerifyHoursWithinLimit(ByVal Optional newTaskHours As Integer = 0) As Boolean
            Return _tasks.Sum(Function(t) t.HoursRemaining) + newTaskHours <= _hoursLimit
        End Function

        Private Sub HandleTaskCompleted(ByVal taskCompletedEvent As TaskCompletedEvent)
            If taskCompletedEvent.Task.ProjectId <> Id Then Exit Sub
            UpdateStatus()
            Log($"{taskCompletedEvent.Task.Name} completed.")
        End Sub

        Private Sub UpdateStatus()
            If Not Tasks.Any() Then
                Status = "New"
                Exit Sub
            End If

            If Tasks.Any(Function(t) t.IsComplete) Then
                Status = "Making Progress"
            End If

            If Tasks.All(Function(t) t.IsComplete) Then
                Status = "Done!"
            End If

            If Tasks.All(Function(t) Not t.IsComplete) Then
                Status = "Not Started"
            End If
        End Sub

        Private Sub Log(ByVal message As String)
            _activityLog.Add(message)
        End Sub

        Public Sub AddTask(ByVal name As String, ByVal hoursRemaining As Integer)
            Dim task = New Task(name, hoursRemaining, Id)

            If hoursRemaining < 0 Then
                Log("Can't add a task with negative hours remaining.")
                Exit Sub
            End If

            If Not VerifyHoursWithinLimit(hoursRemaining) Then
                Log("Can't add a task that will exceed project hours limit.")
                Exit Sub
            End If

            _tasks.Add(task)
            UpdateStatus()
            Log($"{task.Name} added.")
        End Sub

        Public Sub DeleteTask(ByVal id As Guid)
            Dim taskToDelete = _tasks.SingleOrDefault(Function(t) t.Id = id)
            If taskToDelete Is Nothing Then Exit Sub
            _tasks.Remove(taskToDelete)
            AggregateEventsVB.Raise(New TaskDeletedEvent(taskToDelete))
            Log($"{taskToDelete.Name} deleted.")
        End Sub

        Public Overrides Function ToString() As String
            Dim hoursRemaining As Integer = _tasks.Sum(Function(t) t.HoursRemaining)
            Dim sb As StringBuilder = New StringBuilder()
            sb.AppendLine($"Project: {Name} ({Id})")
            sb.AppendLine($"Status: {Status} {hoursRemaining} hours")
            sb.AppendLine("Tasks:")
            sb.AppendLine("--------------")

            For Each task In _tasks
                sb.AppendLine($"Task: {task.Name} {task.HoursRemaining} hours; Complete? {task.IsComplete}")
            Next

            sb.AppendLine("Activity Log:")
            sb.AppendLine("--------------")

            For Each item As String In _activityLog
                sb.AppendLine(item)
            Next

            Return sb.ToString()
        End Function
    End Class
End Namespace
