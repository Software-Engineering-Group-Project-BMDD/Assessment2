# MauiApp1

## This coursework is a .NET MAUI App (.NET 8) that uses real-time data and displays it in a mobile environment.

### By:
- Denver Cavan
- Blair Hunter
- Neil McKenna

### 2025

---

### Project Overview

I have set up the initial data structure using the **MVVM pattern** (Model, ViewModel, Views). The models are split into an external data library for both use in the app and testing.

### Testing Capabilities

The testing capabilities have been set up, though during testing, some errors were thrown. The correct libraries were eventually loaded, but the setup process took longer than expected.

### Navigation

A **navigation button** is included that takes you to a weather page, which displays data fetched from the database.

---

### Database Setup

In the `SQL_DAT_INSERTION_QUERIES` folder, you'll find the SQL database setup and fixture data for the weather functionality.

---

### Code and Output

- The **main code** is compiling and displaying data from the database correctly.
- Testing was throwing errors initially, but once the libraries were set up correctly, everything started working.

---

### Documentation

- I added **DocFX** to generate an HTML output for my classes.
- If you open `_site/index.html` in a browser, you will see the data model, converters, and view models API in the generated HTML website.

---

### Page Layout and Build

To make layout changes to the `index.md` file and update the page layout, you can use the following terminal command:

```bash
docfx build


### Unit test still need created
