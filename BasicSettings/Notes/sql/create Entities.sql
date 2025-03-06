CREATE TABLE ApplicationUser (
    Id BIGINT PRIMARY KEY identity(1, 1),
    UserName NVARCHAR(256) NOT NULL,
    NormalizedUserName NVARCHAR(256) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    NormalizedEmail NVARCHAR(256) NOT NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(15),
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    ModifiedDate DATETIME,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL,
    RegionId INT,
    DistrictId INT,
    RefreshToken NVARCHAR(MAX)
);


CREATE TABLE ApplicantRole (
    Id INT PRIMARY KEY identity(1, 1),
    Name NVARCHAR(256) NOT NULL,
    NormalizedName NVARCHAR(256) NOT NULL,
    IsActive BIT NOT NULL
);


CREATE TABLE UsersRoles (
    UserId BIGINT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES ApplicationUser(Id),
    FOREIGN KEY (RoleId) REFERENCES ApplicantRole(Id)
);


CREATE TABLE SystemTasks (
    Id INT PRIMARY KEY identity(1, 1),
    ParentId INT,
    Name NVARCHAR(500),
    ActionName NVARCHAR(500),
    OrderBy INT,
    Type NVARCHAR(256),
    FOREIGN KEY (ParentId) REFERENCES SystemTasks(Id)
);


CREATE TABLE RoleProfiles (
    TaskId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (TaskId, RoleId),
    FOREIGN KEY (TaskId) REFERENCES SystemTasks(Id),
    FOREIGN KEY (RoleId) REFERENCES ApplicantRole(Id)
);

