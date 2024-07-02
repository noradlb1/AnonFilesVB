Imports System.IO
Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class AnonfilesApiClient
    Private Const BaseUrl As String = "https://api.anonfiles.com"

    Private ReadOnly httpClient As HttpClient

    Public Sub New()
        httpClient = New HttpClient()
    End Sub

    Public Async Function UploadFileAsync(filePath As String) As Task(Of UploadResponse)
        Using formData = New MultipartFormDataContent()
            formData.Add(New StreamContent(New FileStream(filePath, FileMode.Open)), "file", Path.GetFileName(filePath))
            Dim response = Await httpClient.PostAsync($"{BaseUrl}/upload", formData)
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Return JsonConvert.DeserializeObject(Of UploadResponse)(responseBody)
        End Using
    End Function

    Public Async Function GetFileInfoAsync(fileId As String) As Task(Of FileInfoResponse)
        Dim response = Await httpClient.GetAsync($"{BaseUrl}/v2/file/{fileId}/info")
        Dim responseBody = Await response.Content.ReadAsStringAsync()
        Return JsonConvert.DeserializeObject(Of FileInfoResponse)(responseBody)
    End Function
End Class

Public Class UploadResponse
    Public Property Status As Boolean
    Public Property Data As UploadResponseData
    Public Property [Error] As [Error]
End Class

Public Class UploadResponseData
    Public Property File As UploadedFile
End Class

Public Class UploadedFile
    Public Property Url As FileUrl
    Public Property Metadata As FileMetadata
End Class

Public Class FileUrl
    Public Property Full As String
    Public Property [Short] As String
End Class

Public Class FileMetadata
    Public Property Id As String
    Public Property Name As String
    Public Property Size As FileSize
End Class

Public Class FileSize
    Public Property Bytes As Integer
    Public Property Readable As String
End Class

Public Class FileInfoResponse
    Public Property Status As Boolean
    Public Property Data As FileInfoResponseData
    Public Property [Error] As [Error]
End Class

Public Class FileInfoResponseData
    Public Property File As UploadedFile
End Class

Public Class [Error]
    Public Property Message As String
    Public Property Type As String
    Public Property Code As Integer
End Class
