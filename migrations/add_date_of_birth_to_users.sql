-- Migration Script: Add date_of_birth column to users table
-- This migration adds a new column 'date_of_birth' of type DATE which allows NULL values to gracefully support existing records
// REQ-101: Migration adds dateOfBirth column to support new user attribute

ALTER TABLE users
ADD COLUMN date_of_birth DATE NULL;
