# POC - Logging Behavior with Nested Async Tasks in .NET

## Overview
This proof-of-concept (POC) project demonstrates how logging behaves in a multithreaded .NET application, particularly in scenarios involving nested asynchronous tasks. The goal is to highlight a common logging issue encountered when exceptions are aggregated and improperly logged, especially when relying on a logging mechanism that does not correctly process inner exceptions.

## Context
In certain production systems, when asynchronous tasks are nested and executed in parallel, exceptions are often captured as an `AggregateException`. If these are not flattened and iterated correctly, the root causes of errors can be hidden or misrepresented in logs. This POC uses a simplified console application to emulate the structure and behavior of a real web application without disclosing any company-specific details.

## Branches
- **logging-original-behavior**: Contains the original implementation that mirrors the existing issue in the current system.
- **logging-improved-visibility**: Contains an improved version that properly handles and logs nested exceptions for better traceability and debugging. The key goal was not to modify the original application logic�given its complexity and entanglement�but to enhance the visibility and quality of logs without altering functional behavior.

## Before vs After

Switching between the two branches produces the same nested-task failure, logged two very different ways.

### Before (this branch, `logging-original-behavior`)
`AppJob` catches the exception from `Task.WaitAll` and logs only `exception.InnerException.Message`. Because the real failure is wrapped in nested `AggregateException`s, that message is just the generic wrapper text — the actual errors thrown by `Send` never make it into the log:

```
Exception thrown: One or more errors occurred..
```

The full captured output, including the real (buried) inner exceptions that this log line hides, is in [`current-expected-log.txt`](./current-expected-log.txt).

### After (`logging-improved-visibility` branch)
The same failure is caught as an `AggregateException`, flattened with `.Flatten()`, and each inner exception is logged on its own:

```
System.Exception: Error from child 1
System.Exception: Error from child 2
```

The full output — every real exception with its own message and stack trace — is in `fixed-log.txt` on that branch.

### Why this matters
Nested `Task`/`Task.WaitAll` calls wrap failures in layer after layer of `AggregateException`. Logging only `ex.Message` (or an unflattened `ex.InnerException.Message`) silently discards the actual root cause: instead of the real exception, whoever reads the log just sees "One or more errors occurred." Flattening before logging is a one-line fix that turns a useless log entry into an actionable one.

## Structure
- `Program.cs`: Entry point and core simulation of the task execution and error handling logic.
- `AppJob`, `Split`, `LoopAndSend`, `Send`: Simulated job processing chain, mimicking real-world nested task behavior.

## Key Concepts Demonstrated
- Use of `Task.Run` and `Task.WaitAll` for parallel execution.
- Proper handling of `AggregateException` using `Flatten()`.
- Differences between `throw exception;` and `throw;` in async contexts.
- Logging behavior impact when inner exceptions are ignored or mislogged.

## Purpose
This repository is intended to demonstrate and fix a specific logging issue in asynchronous .NET applications involving nested tasks. It serves to document the problem and its resolution, providing a clear and reproducible reference. The goal is to share conclusions, support internal analysis, and help others identify and resolve similar issues by studying this example.

---
Feel free to open the individual branches to compare the logging behavior and exception visibility between the original and corrected implementations.

