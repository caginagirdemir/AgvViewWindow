using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AgvViewWindow.Db;

public partial class BoluRequestContext : DbContext
{
    public BoluRequestContext()
    {
    }

    public BoluRequestContext(DbContextOptions<BoluRequestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agvgroup> Agvgroups { get; set; }

    public virtual DbSet<Agvinfo> Agvinfos { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Errorsetting> Errorsettings { get; set; }

    public virtual DbSet<Errorstop> Errorstops { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationGroup> StationGroups { get; set; }

    public virtual DbSet<Supermarket> Supermarkets { get; set; }

    public virtual DbSet<Symbolicpointhold> Symbolicpointholds { get; set; }

    public virtual DbSet<Symbolicpointinfo> Symbolicpointinfos { get; set; }

    public virtual DbSet<Symbolicpointname> Symbolicpointnames { get; set; }

    public virtual DbSet<Target> Targets { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=boluTest;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agvgroup>(entity =>
        {
            entity.ToTable("agvgroup");

            entity.Property(e => e.AgvGroupId).HasColumnName("agv_group_id");
            entity.Property(e => e.AgvGroupName)
                .HasMaxLength(100)
                .HasColumnName("agv_group_name");
        });

        modelBuilder.Entity<Agvinfo>(entity =>
        {
            entity.HasKey(e => e.AgvId);

            entity.ToTable("agvinfo");

            entity.Property(e => e.AgvId)
                .ValueGeneratedNever()
                .HasColumnName("agv_id");
            entity.Property(e => e.AgvGroupName)
                .HasMaxLength(100)
                .HasColumnName("agv_group_name");
            entity.Property(e => e.AgvName)
                .HasMaxLength(100)
                .HasColumnName("agv_name");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("delivery");

            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.AtPoleTime).HasColumnName("at_pole_time");
            entity.Property(e => e.AtSupermarketTime).HasColumnName("at_supermarket_time");
            entity.Property(e => e.AvgCycleSpeed).HasColumnName("avg_cycle_speed");
            entity.Property(e => e.DeliveredTargetId).HasColumnName("delivered_target_id");
            entity.Property(e => e.DropTime).HasColumnName("drop_time");
            entity.Property(e => e.LastAtPoleTime).HasColumnName("last_at_pole_time");
            entity.Property(e => e.LastAtSupermarketTime).HasColumnName("last_at_supermarket_time");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.PrereleasePointTime).HasColumnName("prerelease_point_time");
            entity.Property(e => e.RequestTime).HasColumnName("request_time");
            entity.Property(e => e.ResponseTime).HasColumnName("response_time");
            entity.Property(e => e.SupermarketId).HasColumnName("supermarket_id");
            entity.Property(e => e.TakePointId).HasColumnName("take_point_id");
            entity.Property(e => e.ToMarketTime).HasColumnName("to_market_time");
        });

        modelBuilder.Entity<Errorsetting>(entity =>
        {
            entity.HasKey(e => e.ErrorId);

            entity.ToTable("errorsettings");

            entity.Property(e => e.ErrorId)
                .ValueGeneratedNever()
                .HasColumnName("error_id");
            entity.Property(e => e.DashboardVisible).HasColumnName("dashboard_visible");
            entity.Property(e => e.Duration)
                .HasDefaultValueSql("((30))")
                .HasColumnName("duration");
            entity.Property(e => e.ErrorGroup)
                .HasMaxLength(50)
                .HasColumnName("error_group");
            entity.Property(e => e.ErrorName)
                .HasMaxLength(100)
                .HasColumnName("error_name");
            entity.Property(e => e.RecordFlag).HasColumnName("record_flag");
            entity.Property(e => e.ShortList)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("short_list");
        });

        modelBuilder.Entity<Errorstop>(entity =>
        {
            entity.HasKey(e => e.ErrorStopId).HasName("PK_errorStops");

            entity.ToTable("errorstops");

            entity.Property(e => e.ErrorStopId).HasColumnName("error_stop_id");
            entity.Property(e => e.ErrorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("error_id");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.NavitrolState).HasColumnName("navitrol_state");
            entity.Property(e => e.PosX).HasColumnName("pos_x");
            entity.Property(e => e.PosY).HasColumnName("pos_y");
            entity.Property(e => e.StopDuration)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stop_duration");
            entity.Property(e => e.TrackBeginTime).HasColumnName("track_begin_time");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("material");

            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.MaterialName)
                .HasMaxLength(100)
                .HasColumnName("material_name");
            entity.Property(e => e.StationGroupId).HasColumnName("station_group_id");
            entity.Property(e => e.StationGroupName)
                .HasMaxLength(100)
                .HasColumnName("station_group_name");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.StationName)
                .HasMaxLength(100)
                .HasColumnName("station_name");
            entity.Property(e => e.SupermarketId).HasColumnName("supermarket_id");
            entity.Property(e => e.SupermarketName)
                .HasMaxLength(100)
                .HasColumnName("supermarket_name");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ManuelRequestStatus).HasColumnName("manuel_request_status");
            entity.Property(e => e.RequestStatus).HasColumnName("request_status");
            entity.Property(e => e.RequestTime).HasColumnName("request_time");
            entity.Property(e => e.RequestedMaterialId).HasColumnName("requested_material_id");
            entity.Property(e => e.RequestedMaterialName)
                .HasMaxLength(100)
                .HasColumnName("requested_material_name");
            entity.Property(e => e.ResponseTime).HasColumnName("response_time");
            entity.Property(e => e.StationGroupId).HasColumnName("station_group_id");
            entity.Property(e => e.StationGroupName)
                .HasMaxLength(100)
                .HasColumnName("station_group_name");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.StationName)
                .HasMaxLength(100)
                .HasColumnName("station_name");
            entity.Property(e => e.SupermarketId).HasColumnName("supermarket_id");
            entity.Property(e => e.SupermarketName)
                .HasMaxLength(100)
                .HasColumnName("supermarket_name");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.ToTable("station");

            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.StationGroupId).HasColumnName("station_group_id");
            entity.Property(e => e.StationGroupName)
                .HasMaxLength(100)
                .HasColumnName("station_group_name");
            entity.Property(e => e.StationName)
                .HasMaxLength(100)
                .HasColumnName("station_name");
            entity.Property(e => e.StationSymbolicPointId).HasColumnName("station_symbolic_point_id");
            entity.Property(e => e.StationSymbolicPrePoint).HasColumnName("station_symbolic_pre_point");
        });

        modelBuilder.Entity<StationGroup>(entity =>
        {
            entity.ToTable("station_group");

            entity.Property(e => e.StationGroupId).HasColumnName("station_group_id");
            entity.Property(e => e.StationGroupName)
                .HasMaxLength(100)
                .HasColumnName("station_group_name");
            entity.Property(e => e.SupermarketId).HasColumnName("supermarket_id");
        });

        modelBuilder.Entity<Supermarket>(entity =>
        {
            entity.ToTable("supermarket");

            entity.Property(e => e.SupermarketId).HasColumnName("supermarket_id");
            entity.Property(e => e.SupermarketName)
                .HasMaxLength(100)
                .HasColumnName("supermarket_name");
            entity.Property(e => e.SupermarketSymbolicPoint).HasColumnName("supermarket_symbolic_point");
            entity.Property(e => e.SupermarketSymbolicPrePoint)
                .HasDefaultValueSql("((1))")
                .HasColumnName("supermarket_symbolic_pre_point");
        });

        modelBuilder.Entity<Symbolicpointhold>(entity =>
        {
            entity.HasKey(e => e.SymbolicPointId);

            entity.ToTable("symbolicpointhold");

            entity.Property(e => e.SymbolicPointId)
                .ValueGeneratedNever()
                .HasColumnName("symbolic_point_id");
            entity.Property(e => e.AtOneLocation).HasColumnName("at_one_location");
            entity.Property(e => e.AtThreeLocation).HasColumnName("at_three_location");
            entity.Property(e => e.AtTwoLocation).HasColumnName("at_two_location");
            entity.Property(e => e.DefaultReleaseLocation).HasColumnName("default_release_location");
            entity.Property(e => e.MachineTypeId).HasColumnName("machine_type_id");
            entity.Property(e => e.ReleaseTimeoutSecond).HasColumnName("release_timeout_second");
        });

        modelBuilder.Entity<Symbolicpointinfo>(entity =>
        {
            entity.HasKey(e => e.SymbolicpointId);

            entity.ToTable("symbolicpointinfo");

            entity.Property(e => e.SymbolicpointId)
                .ValueGeneratedNever()
                .HasColumnName("symbolicpoint_id");
            entity.Property(e => e.PosX).HasColumnName("pos_x");
            entity.Property(e => e.PosY).HasColumnName("pos_y");
        });

        modelBuilder.Entity<Symbolicpointname>(entity =>
        {
            entity.HasKey(e => e.SymbolicPointId);

            entity.ToTable("symbolicpointname");

            entity.Property(e => e.SymbolicPointId)
                .ValueGeneratedNever()
                .HasColumnName("symbolic_point_id");
            entity.Property(e => e.SymbolicPointName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("symbolic_point_name");
            entity.Property(e => e.SymbolicPointType).HasColumnName("symbolic_point_type");
        });

        modelBuilder.Entity<Target>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("target");

            entity.Property(e => e.AgvOptimumSpeed).HasColumnName("agvOptimumSpeed");
            entity.Property(e => e.AgvUtilRatio).HasColumnName("agvUtilRatio");
            entity.Property(e => e.BatteryLevel).HasColumnName("batteryLevel");
            entity.Property(e => e.SmLoad).HasColumnName("smLoad");
            entity.Property(e => e.SmWaiting).HasColumnName("smWaiting");
            entity.Property(e => e.WsWaiting).HasColumnName("wsWaiting");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("zone");

            entity.Property(e => e.ZoneId).HasColumnName("zone_id");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(100)
                .HasColumnName("zone_name");
            entity.Property(e => e.ZoneXOne).HasColumnName("zone_x_one");
            entity.Property(e => e.ZoneXZero).HasColumnName("zone_x_zero");
            entity.Property(e => e.ZoneYOne).HasColumnName("zone_y_one");
            entity.Property(e => e.ZoneYZero).HasColumnName("zone_y_zero");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
