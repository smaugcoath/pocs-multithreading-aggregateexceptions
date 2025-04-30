# POC - Logging Behavior with Nested Async Tasks in .NET

## Overview
This proof-of-concept (POC) project demonstrates how logging behaves in a multithreaded .NET application, particularly in scenarios involving nested asynchronous tasks. The goal is to highlight a common logging issue encountered when exceptions are aggregated and improperly logged, especially when relying on a logging mechanism that does not correctly process inner exceptions.

## Context
In certain production systems, when asynchronous tasks are nested and executed in parallel, exceptions are often captured as an `AggregateException`. If these are not flattened and iterated correctly, the root causes of errors can be hidden or misrepresented in logs. This POC uses a simplified console application to emulate the structure and behavior of a real web application without disclosing any company-specific details.

## Branches
- **logging-original-behavior**: Contains the original implementation that mirrors the existing issue in the current system.
- **logging-improved-visibility**: Contains an improved version that properly handles and logs nested exceptions for better traceability and debugging. The key goal was not to modify the original application logic—given its complexity and entanglement—but to enhance the visibility and quality of logs without altering functional behavior.

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

