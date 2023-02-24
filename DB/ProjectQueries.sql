create database CI_Platform;
use CI_Platform

--admin table
create table admin(
admin_id bigint not null identity(1,1) Primary key ,
first_name varchar(16),
last_name varchar(16),
email varchar(128) not null,
password varchar(255) not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
);

--banner table
create table banner(
banner_id bigint not null identity(1,1),
image varchar(512) not null,
text text,
sort_order int default 0,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--city table
create table city(
city_id bigint not null identity(1,1) primary key,
country_id bigint not null foreign key references country(country_id),
name varchar(255) not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--country table
create table country(
country_id bigint not null identity(1,1) primary key,
name varchar(255) not null,
ISO varchar(16),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--comment table  
create table comment(
comment_id bigint not null identity(1,1) primary key,
user_id bigint not null foreign key references [user](user_id),
mission_id bigint not null foreign key references mission(mission_id),
approval_status varchar(20) check(approval_status in ('PUBLISHED','PENDING','REJECTED')) DEFAULT 'PENDING',
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--cms_page tble
create table cms_table(
cms_page_id bigint not null identity(1,1) primary key,
title varchar(255),
description text,
slug varchar(255) not null,
status int check(status in (0,1)) default 1,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--favorite_mission table 
create table favorite_mission(
favourite_mission_id bigint not null identity(1,1) primary key,
user_id bigint not null foreign key references [user](user_id),
mission_id bigint not null foreign key references mission(mission_id),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--goal_mission table 
create table goal_mission(
goal_mission_id bigint not null identity(1,1) primary key,
mission_id bigint not null foreign key references mission(mission_id),
goal_objective_text varchar(255) default 'Null',
goal_value int not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission table  
create table mission(
mission_id bigint not null identity(1,1) primary key,
theme_id bigint not null foreign key references mission_theme(mission_theme_id),
city_id bigint not null foreign key references city(city_id) ,
country_id bigint not null foreign key references country(country_id),
title varchar(128) not null,
short_description text,
description text default 'null',
start_date datetime default 'null',
end_date datetime default 'null',
mission_type varchar(10) check(mission_type in ('TIME', 'GOAL')) NOT NULL,
status int check(status in (0,1)),
organization_name varchar(255) default 'null',
organization_detail text default 'null',
availability VARCHAR(10) check(availability in ('WEEKLY','DAILY')) default 'null',
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission theme table
create table mission_theme(
mission_theme_id bigint not null identity(1,1) primary key,
title varchar(255),
status tinyint not null default 1,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission application table 
create table mission_application(
mission_application_id bigint not null identity(1,1) primary key,
mission_id bigint not null foreign key references mission(mission_id),
user_id bigint not null foreign key references [user](user_id),
applied_at datetime not null,
approval_status varchar(20) check(approval_status in ('PUBLISHED','PENDING','DECLINED')) DEFAULT 'PENDING',
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission document table
create table mission_document(
mission_document_id bigint not null identity(1,1) primary key,
mission_id bigint not null foreign key references mission(mission_id),
document_name varchar(255),
document_path varchar(255),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission invite table 
create table mission_invite(
mission_invite_id bigint not null identity(1,1) primary key,
mission_id bigint not null foreign key references mission(mission_id),
from_user_id bigint not null foreign key references [user](user_id),
to_user_id bigint not null foreign key references [user](user_id),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission media table
create table mission_media(
mission_media_id bigint not null identity(1,1) primary key,
mission_id bigint not null foreign key references mission(mission_id),
media_name varchar(64),
media_type varchar(4),
media_path varchar(255),
default_media int check(default_media in (0,1)) default 0,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission rating table 
create table mission_rating(
mission_rating_id bigint not null identity(1,1) primary key,
user_id bigint not null foreign key references [user](user_id),
mission_id bigint not null foreign key references mission(mission_id),
rating int check(rating in (1,2,3,4,5)) not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--mission skill 
create table mission_skill(
mission_skill_id bigint not null identity(1,1) primary key,
skill_id int not null foreign key references skill(skill_id),
mission_id bigint foreign key references mission(mission_id),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--password reset
create table password_reset(
email varchar(191) not null,
token varchar(191) not null,
created_at datetime default current_timestamp
)

--skill table
create table skill(
skill_id int not null identity(1,1) primary key,
skill_name varchar(64),
status tinyint not null default 1,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)


--story 
create table story(
story_id  bigint not null identity(1,1) primary key,
user_id bigint not null foreign key references [user](user_id),
mission_id bigint not null foreign key references mission(mission_id),
title varchar(255) default'null',
description text default 'null',
status varchar(10) check(status in ('DECLINED', 'DRAFT')) NOT NULL default 'DRAFT',
published_at datetime default 'null',
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--story_invite
create table story_invite(
story_invite_id  bigint not null identity(1,1) primary key,
story_id bigint not null ,
from_user_id bigint not null ,
to_user_id bigint not null ,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--story media 
create table story_media(
story_media_id  bigint not null identity(1,1) primary key,
story_id bigint not null foreign key references story(story_id),
type varchar(8) not null,
path text not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--timesheet 
create table timesheet(
timesheet_id bigint not null identity(1,1) primary key,
user_id bigint foreign key references [user](user_id),
mission_id bigint foreign key references mission(mission_id),
time time,
action int,
date_volunteered datetime,
notes text,
status varchar(10) check(status in ('APPROVED', 'PENDING')) NOT NULL default 'PENDING',
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)

--USER
create table [user](
user_id bigint not null identity(1,1) primary key,
first_name varchar(16) default 'null',
last_name varchar(16) default 'null',
email varchar(128) not null,
password varchar(255) not null,
phone_number int not null,
avatar varchar(2048) default 'null',
why_i_volunteer text default 'null',
employee_id varchar(16) default 'null', 
department varchar(16)  default 'null', 
city_id bigint not null foreign key references city(city_id) ,
country_id bigint not null foreign key references country(country_id),
profile_text text,
linked_in_url varchar(255),
title varchar(255),
status int check(status in (0,1)) not null,
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
);

--user skill
create table user_skill(
user_skill_id bigint not null identity(1,1) primary key,
user_id bigint foreign key references [user](user_id),
skill_id int not null foreign key references skill(skill_id),
created_at datetime not null default Current_timestamp,
updated_at datetime,
deleted_at datetime
)