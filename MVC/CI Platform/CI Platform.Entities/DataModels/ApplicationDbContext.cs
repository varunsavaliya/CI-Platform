﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Entities.DataModels;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsTable> CmsTables { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavoriteMission> FavoriteMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionMedium> MissionMedia { get; set; }

    public virtual DbSet<MissionRating> MissionRatings { get; set; }

    public virtual DbSet<MissionSkill> MissionSkills { get; set; }

    public virtual DbSet<MissionTheme> MissionThemes { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<StoryInvite> StoryInvites { get; set; }

    public virtual DbSet<StoryMedium> StoryMedia { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admin__43AA414120946997");

            entity.ToTable("admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("banner");

            entity.Property(e => e.BannerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("banner_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Image)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.SortOrder)
                .HasDefaultValueSql("((0))")
                .HasColumnName("sort_order");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__city__031491A889E4D3F6");

            entity.ToTable("city");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__city__country_id__2645B050");
        });

        modelBuilder.Entity<CmsTable>(entity =>
        {
            entity.HasKey(e => e.CmsPageId).HasName("PK__cms_tabl__B46D5B5207DE0BD7");

            entity.ToTable("cms_table");

            entity.Property(e => e.CmsPageId).HasColumnName("cms_page_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comment__E7957687D6F0FDC3");

            entity.ToTable("comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comment__mission__2739D489");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comment__user_id__57DD0BE4");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__country__7E8CD0551201A955");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ISO");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FavoriteMission>(entity =>
        {
            entity.HasKey(e => e.FavouriteMissionId).HasName("PK__favorite__94E4D8CA1C7B475C");

            entity.ToTable("favorite_mission");

            entity.Property(e => e.FavouriteMissionId).HasColumnName("favourite_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavoriteMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favorite___missi__29221CFB");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteMissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favorite___user___531856C7");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.HasKey(e => e.GoalMissionId).HasName("PK__goal_mis__358E02C710C20250");

            entity.ToTable("goal_mission");

            entity.Property(e => e.GoalMissionId).HasColumnName("goal_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("('Null')")
                .HasColumnName("goal_objective_text");
            entity.Property(e => e.GoalValue).HasColumnName("goal_value");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__goal_miss__missi__2B0A656D");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__mission__B5419AB2F9A24782");

            entity.ToTable("mission");

            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Availability)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("availability");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("('null')")
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("('null')")
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.MissionType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("mission_type");
            entity.Property(e => e.OrganizationDetail)
                .HasDefaultValueSql("('null')")
                .HasColumnType("text")
                .HasColumnName("organization_detail");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("organization_name");
            entity.Property(e => e.ShortDescription)
                .HasColumnType("text")
                .HasColumnName("short_description");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("('null')")
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TotalSeats).HasColumnName("total_seats");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.City).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission__city_id__2CF2ADDF");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission__country__2DE6D218");

            entity.HasOne(d => d.Theme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission__theme_i__2BFE89A6");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.HasKey(e => e.MissionApplicationId).HasName("PK__mission___DF92838A655E93D5");

            entity.ToTable("mission_application");

            entity.Property(e => e.MissionApplicationId).HasColumnName("mission_application_id");
            entity.Property(e => e.AppliedAt)
                .HasColumnType("datetime")
                .HasColumnName("applied_at");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__missi__2EDAF651");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__user___489AC854");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.HasKey(e => e.MissionDocumentId).HasName("PK__mission___E80E0CC866F7EF39");

            entity.ToTable("mission_document");

            entity.Property(e => e.MissionDocumentId).HasColumnName("mission_document_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_name");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_path");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_d__missi__30C33EC3");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.HasKey(e => e.MissionInviteId).HasName("PK__mission___A97ED158B7A37207");

            entity.ToTable("mission_invite");

            entity.Property(e => e.MissionInviteId).HasColumnName("mission_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__from___42E1EEFE");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__missi__31B762FC");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MissionInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__to_us__43D61337");
        });

        modelBuilder.Entity<MissionMedium>(entity =>
        {
            entity.HasKey(e => e.MissionMediaId).HasName("PK__mission___848A78E80DAB1B6A");

            entity.ToTable("mission_media");

            entity.Property(e => e.MissionMediaId).HasColumnName("mission_media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DefaultMedia)
                .HasDefaultValueSql("((0))")
                .HasColumnName("default_media");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MediaName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("media_name");
            entity.Property(e => e.MediaPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("media_path");
            entity.Property(e => e.MediaType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("media_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionMedia)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_m__missi__3493CFA7");
        });

        modelBuilder.Entity<MissionRating>(entity =>
        {
            entity.HasKey(e => e.MissionRatingId).HasName("PK__mission___320E5172AD8D984B");

            entity.ToTable("mission_rating");

            entity.Property(e => e.MissionRatingId).HasColumnName("mission_rating_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__missi__3587F3E0");

            entity.HasOne(d => d.User).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__user___3C34F16F");
        });

        modelBuilder.Entity<MissionSkill>(entity =>
        {
            entity.HasKey(e => e.MissionSkillId).HasName("PK__mission___82712008EC2D83C6");

            entity.ToTable("mission_skill");

            entity.Property(e => e.MissionSkillId).HasColumnName("mission_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__mission_s__missi__3864608B");

            entity.HasOne(d => d.Skill).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_s__skill__37703C52");
        });

        modelBuilder.Entity<MissionTheme>(entity =>
        {
            entity.HasKey(e => e.MissionThemeId).HasName("PK__mission___4925C5AC0FE5D744");

            entity.ToTable("mission_theme");

            entity.Property(e => e.MissionThemeId).HasColumnName("mission_theme_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.ToTable("password_reset");

            entity.Property(e => e.PasswordResetId).HasColumnName("password_reset_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__skill__FBBA83790CDEFEE0");

            entity.ToTable("skill");

            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("skill_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PK__story__66339C56B0A98D8A");

            entity.ToTable("story");

            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("('null')")
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.PublishedAt)
                .HasDefaultValueSql("('null')")
                .HasColumnType("datetime")
                .HasColumnName("published_at");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('DRAFT')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Stories)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__mission_i__395884C4");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__user_id__2A164134");
        });

        modelBuilder.Entity<StoryInvite>(entity =>
        {
            entity.HasKey(e => e.StoryInviteId).HasName("PK__story_in__04497867E278CB52");

            entity.ToTable("story_invite");

            entity.Property(e => e.StoryInviteId).HasColumnName("story_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StoryMedium>(entity =>
        {
            entity.HasKey(e => e.StoryMediaId).HasName("PK__story_me__29BD053C5AECFA38");

            entity.ToTable("story_media");

            entity.Property(e => e.StoryMediaId).HasColumnName("story_media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Path)
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryMedia)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story_med__story__3B40CD36");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__timeshee__7BBF5068EE3301C5");

            entity.ToTable("timesheet");

            entity.Property(e => e.TimesheetId).HasColumnName("timesheet_id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateVolunteered)
                .HasColumnType("datetime")
                .HasColumnName("date_volunteered");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("status");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__timesheet__missi__3C34F16F");

            entity.HasOne(d => d.User).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__timesheet__user___208CD6FA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user__B9BE370F6F85929D");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("avatar");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('null')")
                .HasColumnName("last_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("linked_in_url");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("profile_text");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasDefaultValueSql("('null')")
                .HasColumnType("text")
                .HasColumnName("why_i_volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__user__city_id__160F4887");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__user__country_id__17036CC0");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__user_ski__FD3B576BC4F8975F");

            entity.ToTable("user_skill");

            entity.Property(e => e.UserSkillId).HasColumnName("user_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_skil__skill__40058253");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__user_skil__user___1BC821DD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
