# Log Parser Benchmark Comparison

Date: 2026-04-24

## Scope

This report compares the current benchmark results of three log parser implementations:

- LogParser_Rust
- LogParser_Zig
- LogParser_DotNet10

Each implementation processed `100,000,000` log lines.

## Benchmark Results

| Project | Method | Execution Time (ms) | Memory Usage (MB) | Lines Processed | Runtime Version |
|---|---|---:|---:|---:|---|
| LogParser_Rust | Memory_Mapped | 1380 | 38.0 | 100,000,000 | 1.78 |
| LogParser_Zig | Manual_Chunking | 1420 | 45.5 | 100,000,000 | v0.13.0 |
| LogParser_DotNet10 | Buffered_Read | 1850 | 120.2 | 100,000,000 | 10.0.100 |

## Throughput

| Project | Approx. Throughput |
|---|---:|
| LogParser_Rust | 72.46 million lines/sec |
| LogParser_Zig | 70.42 million lines/sec |
| LogParser_DotNet10 | 54.05 million lines/sec |

## Comparative Analysis

### Rust vs Zig

- Rust is the fastest implementation in the current benchmark.
- Zig is very close to Rust, trailing by `40 ms`.
- Rust is approximately `2.8%` faster than Zig.
- Rust uses `7.5 MB` less memory than Zig.
- Rust uses approximately `16.5%` less memory than Zig.

### Rust vs .NET 10

- Rust is approximately `34.1%` faster than .NET 10.
- .NET 10 uses approximately `3.16x` more memory than Rust.

### Zig vs .NET 10

- Zig is approximately `30.3%` faster than .NET 10.
- .NET 10 uses approximately `2.64x` more memory than Zig.

## Interpretation

These results should be interpreted as a comparison of the current implementations and their file-processing strategies, not only of the programming languages themselves.

- Rust uses `Memory_Mapped`
- Zig uses `Manual_Chunking`
- .NET 10 uses `Buffered_Read`

This means the benchmark reflects both language/runtime characteristics and I/O strategy decisions.

## Ranking

1. LogParser_Rust
2. LogParser_Zig
3. LogParser_DotNet10

## Conclusion

Rust currently provides the strongest overall result, combining the best execution time and the lowest memory usage. Zig is highly competitive and remains very close to Rust. The .NET 10 implementation is functionally viable, but in this benchmark it is meaningfully behind both Rust and Zig in speed and memory efficiency.
