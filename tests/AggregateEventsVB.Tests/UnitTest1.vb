Imports AggregateEventsVB.Model

<TestClass()>
Public Class UnitTest1

    <TestMethod()>
    Public Sub HasStatusNew()
        Dim project = New Project()
        Assert.AreEqual("New", project.Status)
    End Sub

    <TestMethod()>
    Public Sub HasZeroTasks()
        Dim project = New Project()
        Assert.IsFalse(project.Tasks.Any)
    End Sub

    <TestMethod()>
    Public Sub HandleOwnTaskCompletedEventOnly()
        ClearCallbacks()
        Dim project = New Project()
        Dim taskName As String = Guid.NewGuid().ToString()
        project.AddTask(taskName, 1)
        Dim project2 = New Project()
        Raise(New TaskCompletedEvent(project.Tasks.First()))
        Assert.IsTrue(project.ToString().Contains(taskName))
        Assert.IsFalse(project2.ToString().Contains(taskName))
    End Sub

    <TestMethod()>
    Public Sub HandleOwnTaskHoursUpdatedEventOnly()
            AggregateEvents.ClearCallbacks()
            Dim project = New Project()
            Dim taskName As String = Guid.NewGuid().ToString()
            project.AddTask(taskName, 1)
            Dim project2 = New Project()
        project.Tasks.First().UpdateHoursRemaining(2)
        Assert.IsTrue(project.ToString().Contains(taskName))
        Assert.IsFalse(project2.ToString().Contains(taskName))
    End Sub

End Class