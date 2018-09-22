Imports System

Namespace Model
    Public Class Task
        Inherits Entity

        Public Sub New(taskName As String, taskHoursRemaining As Integer, theProjectId As Guid)
            Name = taskName
            HoursRemaining = taskHoursRemaining
            ProjectId = theProjectId
        End Sub

        Private Sub New()
        End Sub

        Public ReadOnly Property ProjectId As Guid

        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Private Set(value As String)
                _Name = value
            End Set
        End Property 'get / private set

        Private _IsComplete As Boolean
        Public Property IsComplete As Boolean
            Get
                Return _IsComplete
            End Get
            Private Set(value As Boolean)
                _IsComplete = value
            End Set
        End Property 'get / private set

        Private _HoursRemaining As Integer
        Public Property HoursRemaining As Integer
            Get
                Return _HoursRemaining
            End Get
            Private Set(value As Integer)
                _HoursRemaining = value
            End Set
        End Property 'get / private set

        Public Sub MarkComplete()
            If IsComplete Then Exit Sub
            IsComplete = True
            HoursRemaining = 0
            AggregateEvents.Raise(New TaskCompletedEvent(Me))
        End Sub

        Public Sub UpdateHoursRemaining(ByVal hours As Integer)
            If hours < 0 Then Exit Sub
            Dim currentHoursRemaining As Integer = HoursRemaining

            Try
                HoursRemaining = hours

                If HoursRemaining = 0 Then
                    MarkComplete()
                    Exit Sub
                End If

                IsComplete = False
                AggregateEvents.Raise(New TaskHoursUpdatedEvent(Me))

            Catch ex As Exception
                HoursRemaining = currentHoursRemaining
            End Try

        End Sub
    End Class
End Namespace
