# MStestNotFoundRepro

Demonstrates cause of the 'tests not found' bug
[Bug report on DevCommunity](https://developercommunity.visualstudio.com/content/problem/671226/unit-tests-not-found-filter-problem-on-newly-upgra.html)

## Background

We have a library that has several test cases we want to reuse in different projects. The test cases have
`Assert.[something]` statements so we needed to reference MSTestV2. The project is a library using .NET Framework 4.7.2.

We created a `nuget` package for this project (our internal feed) and have been using it with no issues.

## Problem
When we upgraded to Visual Studio 2019 version 16.2 a problem arose. The projects using the test library still compiled fine, 
and the unit tests were all visible. However whenever we ran them in Visual Studio via the Test Explorer window, nothing happened.

In the _Output Window_ in the _Tests_ section, we could see the following:
```
[... Warning] No test matches the given testcase filter ...
```
This is the bug reported (link at the top).

## Cause and Solution

I did some investigation and found that the issue was that the test case library had imported both `MsTest.TestFramework` 
_and_ `MsTest.TestAdapter`. It was this second reference that causes the issue. The TestAdapter is used to find tests - and in
my case the test case library didn't have any tests, and somehow the test SDK in VS 2019 v16.2 was tripping over this issue.

I removed the reference to `MSTest.TestAdapter` from the nuget package, and the issue was fixed.

## Sample Repo

This sample repo demonstrates this bug. It should be run on Visual Studio 2019 version 16.2. 
I also tested on v16.3.0 preview 1.0 and it is evident there also.

The repo has a `HelperLib` (which replicates our test case library), that we build and create a `nuget` package from. 
This has been placed in the `/LocalPackages` folder

We have a `ClassLibrary1` as a test subject, and `UnitTestProject1` which has a project reference to `ClassLibrary1` and a NUGET reference to `HelperLib`.

### Test Steps

1. Ensure the solution builds correctly
2. Open the Test Explorer window
3. Click "Run all tests"
4. Should see nothing happen - tests are still 'blue' (not run)
5. Open `Output` pane and show output from `Tests`
```
[07/08/2019 3:30:47.343 PM Informational] ========== Discovery skipped: All test containers are up to date ==========
[07/08/2019 3:30:47.348 PM Informational] ---------- Run started ----------
[07/08/2019 3:30:48.495 PM Warning] No test matches the given testcase filter `FullyQualifiedName=UnitTestProject1.UnitTest1.TestMethod1` in C:\Users\Howard\Source\Repos\MStestNotFoundRepro\UnitTestProject1\bin\Debug\net472\UnitTestProject1.dll
[07/08/2019 3:30:48.736 PM Informational] ========== Run finished: 0 tests run (0:00:01.2964078) ==========
```
