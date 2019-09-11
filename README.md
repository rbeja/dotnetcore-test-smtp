# dotnetcore-test-smtp
Simple app based on .Net Core to test SMTP Server from command line

## Build and test

```
dotnet build
dotnet run --server smtp.office365.com --port 25 -u email@email.com -p password--subject hello --body world --to email@email.com
```
