Imports DevExpress.Mvvm
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Common
Imports System.Data.Entity
Imports System.Data.Entity.ModelConfiguration
Imports System.Data.SQLite
Imports System.Linq
Imports System.Threading.Tasks

Namespace DocumentPrintPreview
	Public Class Employee
		Public Property EmployeeId() As Integer
		Public Property FirstName() As String
		Public Property LastName() As String
		Public Property Title() As String
	End Class
	Public Module NWindDataProvider
		Public ReadOnly Property Employees() As IList(Of Employee)
			Get
				Return DirectCast((New NWindDataLoader()).Employees, IList(Of Employee))
			End Get
		End Property
	End Module
	Public Class NWindDataLoader
		Private context As NWindContext
		Public Sub New()
			If Not ViewModelBase.IsInDesignMode Then
				context = NWindContext.Create()
			End If
		End Sub
		Public ReadOnly Property Employees() As Object
			Get
				If ViewModelBase.IsInDesignMode Then
					Return New List(Of Employee)()
				End If
				context.Employees.Load()
				Return context.Employees.Local
			End Get
		End Property
	End Class
	Partial Public Class NWindContext
		Inherits DbContext

		Public Sub New()
			MyBase.New(CreateConnection(), True)
		End Sub
		Public Sub New(ByVal connectionString As String)
			MyBase.New(connectionString)
		End Sub
		Public Sub New(ByVal connection As DbConnection)
			MyBase.New(connection, True)
		End Sub

		Shared Sub New()
			Database.SetInitializer(Of NWindContext)(Nothing)
		End Sub

		Private Shared Function CreateConnection() As DbConnection
			Dim connection = DbProviderFactories.GetFactory("System.Data.SQLite.EF6").CreateConnection()
			connection.ConnectionString = New SQLiteConnectionStringBuilder With {.DataSource = "|DataDirectory|\nwind.db"}.ConnectionString
			Return connection
		End Function

		Public Overrides Function SaveChanges() As Integer
			Throw New Exception("Readonly context")
		End Function

		Public Shared Function Load() As Task
			Dim action As Action = Sub()
				Dim context = New NWindContext()
				Dim prop = GetType(NWindContext).GetProperties().Where(Function(p) p.PropertyType.IsGenericType AndAlso p.PropertyType.GetGenericTypeDefinition() Is GetType(DbSet(Of ))).FirstOrDefault()
				If prop Is Nothing Then
					Return
				End If
				Dim query = DirectCast(prop.GetValue(context, Nothing), IQueryable(Of Object))
				query.Count()
			End Sub
			Return (New TaskFactory()).StartNew(action)
		End Function

		Public Shared Function Create() As NWindContext
			Load().Wait()
			Return New NWindContext()
		End Function

		Public Property Employees() As DbSet(Of Employee)

		Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
			modelBuilder.Configurations.Add(New EmployeeMap())
		End Sub
	End Class
	Public Class EmployeeMap
		Inherits EntityTypeConfiguration(Of Employee)

		Public Sub New()
			HasKey(Function(t) t.EmployeeId)
			[Property](Function(t) t.EmployeeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
			[Property](Function(t) t.LastName).IsRequired().HasMaxLength(20)
			[Property](Function(t) t.FirstName).IsRequired().HasMaxLength(10)
			[Property](Function(t) t.Title).HasMaxLength(30)
			ToTable("Employees")
			[Property](Function(t) t.EmployeeId).HasColumnName("EmployeeId")
			[Property](Function(t) t.LastName).HasColumnName("LastName")
			[Property](Function(t) t.FirstName).HasColumnName("FirstName")
			[Property](Function(t) t.Title).HasColumnName("Title")
		End Sub
	End Class
End Namespace
