using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var postgresUser = builder.AddParameter("postgres-user");
var postgresPassword = builder.AddParameter("postgres-password");

var pgBuilder = builder.AddPostgres("postgres-identity", port: 5432, userName: postgresUser, password: postgresPassword)
    .WithImage("postgres:latest");

var postgresIdentity = pgBuilder
    .AddDatabase("identitydb");
var postgresQuestion = pgBuilder
    .AddDatabase("questiondb");
var postgresStudent = pgBuilder
    .AddDatabase("studentdb");
var postgresChecking = pgBuilder
    .AddDatabase("checkingdb");
var postgresFiles = pgBuilder
    .AddDatabase("filesdb");
var postgresNotif = pgBuilder
    .AddDatabase("notificationdb");

var rabbitmq = builder.AddRabbitMQ("rabbitmq", port: 5672);

var redis = builder.AddRedis("redis", port: 6379);

var identityService = builder.AddProject<IdentityService_WebApi>("identity-service")
    .WithReference(postgresIdentity)
    .WithReference(rabbitmq)
    .WaitFor(postgresIdentity);

var questionService = builder.AddProject<QuestionManagementService_WebApi>("question-service")
    .WithReference(postgresQuestion)
    .WithReference(rabbitmq)
    .WaitFor(postgresQuestion);

var studentService = builder.AddProject<StudentManagementService_WebApi>("student-service")
    .WithReference(postgresStudent)
    .WithReference(rabbitmq)
    .WaitFor(postgresStudent);

var checkingService = builder.AddProject<TestCheckingService_WebApi>("checking-service")
    .WithReference(postgresChecking)
    .WithReference(rabbitmq)
    .WaitFor(postgresChecking);

var fileService = builder.AddProject<FileStorageService_WebApi>("file-service")
    .WithReference(postgresFiles)
    .WaitFor(postgresFiles);

var notificationService = builder.AddProject<NotificationService_WebApi>("notification-service")
    .WithReference(postgresNotif)
    .WithReference(rabbitmq)
    .WithReference(redis)
    .WaitFor(postgresNotif);

var bffService = builder.AddProject<BffPortalService_WebApi>("bff-portal-service")
    .WithReference(identityService)
    .WithReference(questionService)
    .WithReference(studentService)
    .WithReference(checkingService)
    .WithReference(fileService)
    .WithReference(notificationService)
    .WithReference(redis)
    .WaitFor(identityService);

var ui = builder.AddNpmApp("ui", "../TestsDelivery.Ui")
    .WithReference(bffService)
    .WaitFor(bffService);

builder.Build().Run();
