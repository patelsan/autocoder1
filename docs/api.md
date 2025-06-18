# API Documentation Update

## User Entity Enhancement

The `User` entity has been updated to include a new `dateOfBirth` attribute as per REQ-101. This change introduces the following updates:

- **Data Model:**
  - The `User` model now includes a new field `DateOfBirth` of type `DateTime?`, which allows null values.
  - The field is annotated with data validation attributes to enforce the ISO 8601 date format (`YYYY-MM-DD`).

- **API Endpoints:**
  - Both the create and update endpoints now accept a `dateOfBirth` value.
  - Input validation has been added to the update endpoint to ensure that any provided date is valid and follows the required format. Error responses provide clear feedback.

- **Database Migration:**
  - A migration script (`migrations/add_date_of_birth_to_users.sql`) has been implemented to alter the users table by adding a nullable `date_of_birth` column, with a default of null for existing records.

- **Tests:**
  - Unit and integration tests have been added in `API.Tests/UserDateOfBirthTests.cs` to cover creation, update validation, and migration behavior regarding the `dateOfBirth` field.

This documentation update ensures that all consumers of the API are aware of the new attribute and its validation rules.