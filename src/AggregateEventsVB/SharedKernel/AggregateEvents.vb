Imports System.Collections.Generic
Imports System.Linq

Public Module AggregateEvents

    '/!\ Dont use ThreadStatic in ASP.NET https://www.hanselman.com/blog/ATaleOfTwoTechniquesTheThreadStaticAttributeAndSystemWebHttpContextCurrentItems.aspx
    <ThreadStatic> 'so that each thread has its own callbacks
    Private _actions As List(Of [Delegate])

    Public Sub Register(Of T As AggregateEvent)(callback As Action(Of T))

        If _actions Is Nothing Then
            _actions = New List(Of [Delegate])()
        End If

        _actions.Add(callback)
    End Sub

    Public Sub ClearCallbacks()

        _actions = Nothing
    End Sub

    Public Sub Raise(Of T As AggregateEvent)(args As T)

        If _actions Is Nothing Then Exit Sub

        For Each anAction In _actions.OfType(Of Action(Of T))
            anAction(args)
        Next

    End Sub

End Module

