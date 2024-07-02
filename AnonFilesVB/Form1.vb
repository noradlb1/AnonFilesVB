Imports System
Imports System.IO
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class Form1
    Inherits Form

    Private anonfilesApiClient As AnonfilesApiClient = New AnonfilesApiClient()
    Private Async Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim filePath As String = txtFilePath.Text

        If Not File.Exists(filePath) Then
            lblMessage.Text = "File not found: " & filePath
            Return
        End If

        Try
            Dim uploadResponse = Await anonfilesApiClient.UploadFileAsync(filePath)

            If uploadResponse.Status Then
                ' File uploaded successfully
                Dim uploadedFile = uploadResponse.Data.File
                lblMessage.Text = "File URL: " & uploadedFile.Url.Full & vbCrLf & "File ID: " & uploadedFile.Metadata.Id
            Else
                ' Handle the error response
                lblMessage.Text = "Error: " & uploadResponse.Error.Message
            End If
        Catch ex As Exception
            lblMessage.Text = "An error occurred: " & ex.Message
        End Try
    End Sub

    Private Async Sub btnRetrieve_Click(sender As Object, e As EventArgs) Handles btnRetrieve.Click
        Dim fileId As String = txtFileId.Text

        Try
            Dim fileInfoResponse = Await anonfilesApiClient.GetFileInfoAsync(fileId)

            If fileInfoResponse.Status Then
                ' File info retrieved successfully
                Dim file = fileInfoResponse.Data.File
                lblMessage.Text = "File URL: " & file.Url.Full & vbCrLf & "File Name: " & file.Metadata.Name & vbCrLf & "File Size: " & file.Metadata.Size.Readable
            Else
                ' Handle the error response
                lblMessage.Text = "Error: " & fileInfoResponse.Error.Message
            End If
        Catch ex As Exception
            lblMessage.Text = "An error occurred: " & ex.Message
        End Try
    End Sub
End Class
