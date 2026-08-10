# Copilot Instructions

## Project Guidelines
- For Cropper.Blazor Cropper.js v2 migration: use the existing Cropper.Blazor architecture, add unit-related tests, update examples, and keep separate v1 and v2 example URLs (/v1 and /v2). Implement VM behavior in client-side examples using existing Cropper.js custom element APIs and boundary-limiting events from cropper-image/cropper-selection docs, not custom core interop logic.
- For the docs site, use Cropper.Blazor v1 from NuGet with the same v1 docs, create separate pages (including home) for v2, and keep the releases page shared between both versions. Ensure that the documentation versioning is handled within one application using different routes, rather than separate client apps.
- Inspect the Blazorise.Cropper project locally when comparing Cropper viewer behavior to ensure consistency and functionality.
- Split large god-object option classes into understandable, element-specific option classes for Cropper.Blazor configuration. Prefer enum-backed option APIs instead of plain strings for related Cropper.js option values.
- Verify that event correlation IDs are used correctly.
- For selection snapshots in the Cropper demo, use Cropper preview/viewer elements instead of saved canvas data URLs and avoid redundant snapshot save logic.

## Development Practices
- Run the Super-Linter pipeline configuration locally before completion to ensure code quality and adherence to standards.
- Use conventional Arrange-Act-Assert naming/structure patterns for tests where applicable.
- Place all TypeScript tests in the Vitest project. The default move handle action should be select, allowing for a selection count of zero.
- All TypeScript tests in this repository should be placed in the Vitest project.