Namespace Model
    Public Class TaskDeletedEvent
        Inherits AggregateEvent

        Public Property myTask As Task

        Public Sub New(aTask As Task)
            myTask = aTask
        End Sub

    End Class

End Namespace