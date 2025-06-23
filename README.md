# Marten V8 Identifiers
A repo to reproduce the breaking change in Marten 8 when using AggregateStreamAsync.

## Problem description
Between Marten 7 and the latest version (8.3), the behavior of ```AggregateStreamAsync``` has broken, when a "non conventional" ID field (not named ID) is used on the Aggregate type.

When using a convetional ID field, the behavior is unchanged.

When using a different ID field, and mapping the identifier in the schema, this breaks ```AggregateStreamAsync```.

## Steps to reproduce
This repo show the difference in behavior between v7 and v8. It contains 2 unit test projects, one for each version, and identical tests on both:
* A test with a conventinal ID
* A test with a differently named ID

On v8, the second test fails.

To run the tests:
1. Get the code in this repo
2. set the appsettings.json files with a connection string to an existing DB
3. Run the test to see the failing test