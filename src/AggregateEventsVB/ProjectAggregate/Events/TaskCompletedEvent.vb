Namespace Model
    Public Class TaskCompletedEvent
        Inherits AggregateEvent

        Public Property Task As Task

        Public Sub New(ByVal aTask As Task)
            Task = aTask
        End Sub
    End Class
End Namespace
