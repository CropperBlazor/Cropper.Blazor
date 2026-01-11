require('webpack');
const path = require('path');
const CleanCSS = require('clean-css')
const CopyPlugin = require('copy-webpack-plugin')
const TerserPlugin = require('terser-webpack-plugin')
const { writeFileSync } = require("fs");
const { generateDtsBundle } = require("dts-bundle-generator");

module.exports = (env, args) => ({
    resolve: {
        extensions: ['.ts', '.js', '.css'],
        alias: {
            cropperjs: path.resolve(__dirname, 'node_modules/cropperjs/src')
        }
    },
    devtool: args.mode === 'development' ? 'inline-source-map' : 'hidden-source-map',
    module: {
        rules: [{
            test: /\.ts?$/,
            loader: 'ts-loader',
            exclude: [
                /node_modules/,
                /\.test\.ts$/,      // exclude test files
                /\.spec\.ts$/,      // exclude spec files
                /vitest\.setup\.ts$/ // exclude Vitest setup
            ]
        }]
    },
    entry: {
        "cropperJsInterop": './Cropper/cropperJsInterop.ts'
    },
    output: {
        path: path.join(__dirname, '/wwwroot'),
        filename: '[name].min.js'
    },
    plugins: [new CopyPlugin({
        patterns: [{
            to: 'cropper.min.css',
            from: path.resolve(__dirname, 'node_modules/cropperjs/dist', 'cropper.min.css'),
            transform: content => (new CleanCSS({
                level: 2
            }).minify(content)).styles
        }]
    }),
        {
            apply: (compiler) => {
                compiler.hooks.afterEmit.tap("GenerateDeclarations", () => {
                    try {
                        const { generateDtsBundle } = require('dts-bundle-generator');
                        const fs = require('fs');
                        const path = require('path');

                        // Input TypeScript entry file
                        const entryFile = path.resolve(__dirname, 'Cropper/cropperJsInterop.ts');

                        // Output bundled .d.ts
                        const outputDir = path.resolve(__dirname, 'wwwroot');
                        // Add any helper files you want to include in the bundle
                        const helperFiles = [
                            path.resolve(__dirname, 'Cropper/helpers/blob-helper.ts'),
                            path.resolve(__dirname, 'Cropper/helpers/cropper-url-image-helper.ts'),
                            // add more helpers here if needed
                        ];

                        // Combine main entry + helpers
                        const filesToBundle = [entryFile, ...helperFiles];

                        // Create folder if it doesn't exist
                        if (!fs.existsSync(outputDir)) {
                            fs.mkdirSync(outputDir, { recursive: true });
                        }

                        // Generate the bundle
                        filesToBundle.forEach(filePath => {
                            const bundledDts = generateDtsBundle(
                                [
                                    {
                                        config: path.resolve(__dirname, 'tsconfig.json'),
                                        filePath,                                  
                                        output: {
                                            inlineDeclareGlobals: true,
                                            noBanner: true,
                                            exportReferencedTypes: true,
                                            sortNodes: true
                                        },
                                    }
                                ]
                            );

                            // Clean up empty exports, relative re-exports, and extra blank lines
                            const cleanedDts = bundledDts[0]
                                .replace(/^export\s*{\s*};?\s*$/gmi, '')      // remove empty exports
                                .replace(/^export\s+\*.*?\bfrom\s+"[\.~\/].*$/gmi, '') // remove relative re-exports
                                .replace(/^\s*[\r\n]+/gmi, '')               // remove empty lines
                                .replace(/\/\/.*$/gmi, '')      // remove // comments
                                .replace(/\/\*[\s\S]*?\*\//gmi, ''); // remove /* */ comments

                            const fileName = path.basename(filePath, path.extname(filePath));
                            const outputFile = path.join(outputDir, fileName + ".d.ts");

                            // Write to disk
                            fs.writeFileSync(outputFile, cleanedDts, 'utf8');

                            console.log('✅ Bundled .d.ts created at:', outputFile);
                        });

                    } catch (error) {
                        console.error("⚠️ Declaration generation failed:", error);

                        throw error;
                    }
                });
            }
        }],
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            terserOptions: {
                format: {
                    comments: false,
                },
            },
            extractComments: false,
        })],
        splitChunks: {
            chunks: 'all', // Split all types of chunks (initial, async)
            cacheGroups: {
                // Group for node_modules (cropper)
                cropper: {
                    test: /[\\/]node_modules[\\/]/, // Match modules in node_modules
                    name: 'cropper', // Output filename: cropper.js
                    chunks: 'all',
                    priority: -10 // Lower priority for cropper, higher for app
                }
            },
        },
    },
});