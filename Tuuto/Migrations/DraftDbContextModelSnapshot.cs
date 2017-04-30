using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tuuto.Model;

namespace Tuuto.Migrations
{
    [DbContext(typeof(DraftDbContext))]
    partial class DraftDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Tuuto.Model.DraftModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<int>("AccountId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Domain");

                    b.Property<string>("ErrorMessage");

                    b.Property<int?>("ReplyStatusId");

                    b.Property<bool>("Sensitive");

                    b.Property<string>("SpoilerText");

                    b.Property<string>("Status");

                    b.Property<string>("Visibility");

                    b.HasKey("Id");

                    b.HasIndex("ReplyStatusId");

                    b.ToTable("Draft");
                });

            modelBuilder.Entity("Tuuto.Model.MediaData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DraftModelId");

                    b.Property<string>("SavedFile");

                    b.HasKey("Id");

                    b.HasIndex("DraftModelId");

                    b.ToTable("MediaData");
                });

            modelBuilder.Entity("Tuuto.Model.ReplyModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acct");

                    b.Property<string>("Avatar");

                    b.Property<string>("Content");

                    b.Property<int>("InReplyToId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Reply");
                });

            modelBuilder.Entity("Tuuto.Model.DraftModel", b =>
                {
                    b.HasOne("Tuuto.Model.ReplyModel", "ReplyStatus")
                        .WithMany()
                        .HasForeignKey("ReplyStatusId");
                });

            modelBuilder.Entity("Tuuto.Model.MediaData", b =>
                {
                    b.HasOne("Tuuto.Model.DraftModel")
                        .WithMany("Medias")
                        .HasForeignKey("DraftModelId");
                });
        }
    }
}
