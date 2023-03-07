INSERT INTO mission (theme_id, city_id, country_id, title, short_description, description, start_date, end_date, mission_type, status, organization_name, organization_detail, availability, created_at, updated_at) VALUES 
  
  (1, 8, 1, 'Teaching in India', 'Teach English to children in a rural village', 'This program offers volunteers the opportunity to teach English to children in a rural village in India.', '2023-08-01 00:00:00', '2023-10-31 00:00:00', 'time', 1, 'Volunteering Solutions', 'We are an international volunteer placement organization providing safe and affordable volunteer programs in developing countries.', 'weekly', GETDATE(), GETDATE())


  select * from mission


  ,