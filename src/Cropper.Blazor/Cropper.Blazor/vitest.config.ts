import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        globals: true,
        environment: 'node',
        include: ['**/*.{test,spec}.{ts,tsx,js,jsx}'],
        isolate: true,
        coverage: {
            provider: 'istanbul',
            enabled: false,
            reportsDirectory: './coverage',
            reporter: ['text', 'html', 'cobertura'],
            exclude: [
                '**/*.d.ts',
                '**/*.spec.*',
                '**/*.test.*',
                'node_modules/**',
                'dist/**'
            ]
        }
    }
});
