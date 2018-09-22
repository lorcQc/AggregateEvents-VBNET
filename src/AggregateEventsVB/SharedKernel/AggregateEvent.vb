Imports System

Public MustInherit Class AggregateEvent

    Private _DateTimeOffset As DateTimeOffset = DateTime.UtcNow
    Public Property DateOccurred As DateTimeOffset
        Get
            Return _DateTimeOffset
        End Get
        Private Set(value As DateTimeOffset)
            _DateTimeOffset = value
        End Set
    End Property

End Class
