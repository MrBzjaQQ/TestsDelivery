﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestsPortal.DAL.Data;

#nullable disable

namespace TestsPortal.DAL.Migrations
{
    [DbContext(typeof(TestsPortalContext))]
    [Migration("20220519015321_AddedInstanceUrl")]
    partial class AddedInstanceUrl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TestsPortal.DAL.Models.Candidates.Candidate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.AnswerOption", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnswerOptions");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.ChoiceAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("AnswerId")
                        .HasColumnType("bigint");

                    b.Property<long>("CandidateId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("ScheduledTestId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("ScheduledTestId");

                    b.ToTable("ChoiceAnswers");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.TextAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CandidateId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("ScheduledTestId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("ScheduledTestId");

                    b.ToTable("TextAnswers");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.ScheduledTests.ScheduledTest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AdminPanelInstance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("TestId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("ScheduledTests");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.ScheduledTests.ScheduledTestInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CandidateId")
                        .HasColumnType("bigint");

                    b.Property<string>("Keycode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ScheduledTestId")
                        .HasColumnType("bigint");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ScheduledTestId");

                    b.ToTable("ScheduledTestInstances");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Subject.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Tests.QuestionInTest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("TestId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TestId");

                    b.ToTable("QuestionInTests");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Tests.Test", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.ChoiceAnswer", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Questions.AnswerOption", "AnswerOption")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.Candidates.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.Questions.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.ScheduledTests.ScheduledTest", "ScheduledTest")
                        .WithMany()
                        .HasForeignKey("ScheduledTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnswerOption");

                    b.Navigation("Candidate");

                    b.Navigation("Question");

                    b.Navigation("ScheduledTest");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.Question", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Subject.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Questions.TextAnswer", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Candidates.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.Questions.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.ScheduledTests.ScheduledTest", "ScheduledTest")
                        .WithMany()
                        .HasForeignKey("ScheduledTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("Question");

                    b.Navigation("ScheduledTest");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.ScheduledTests.ScheduledTest", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Tests.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.ScheduledTests.ScheduledTestInstance", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Candidates.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.ScheduledTests.ScheduledTest", "ScheduledTest")
                        .WithMany()
                        .HasForeignKey("ScheduledTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("ScheduledTest");
                });

            modelBuilder.Entity("TestsPortal.DAL.Models.Tests.QuestionInTest", b =>
                {
                    b.HasOne("TestsPortal.DAL.Models.Questions.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsPortal.DAL.Models.Tests.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Test");
                });
#pragma warning restore 612, 618
        }
    }
}