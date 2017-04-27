using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tuuto.Model;

namespace Tuuto.Migrations
{
    [DbContext(typeof(DraftDbContext))]
    [Migration("20170427090020_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Tuuto.Model.DraftModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<int>("AccountId");

                    b.Property<string>("Domain");

                    b.Property<string>("ErrorMessage");

                    b.Property<int>("InReplyToId");

                    b.Property<bool>("Sensitive");

                    b.Property<string>("SpoilerText");

                    b.Property<string>("Status");

                    b.Property<string>("Visibility");

                    b.HasKey("Id");

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

            modelBuilder.Entity("Tuuto.Model.MediaData", b =>
                {
                    b.HasOne("Tuuto.Model.DraftModel")
                        .WithMany("Medias")
                        .HasForeignKey("DraftModelId");
                });
        }
    }
}
