# Build and package

`Directory.Build.props` owns the mod version. Bump it for each port.
Every normal build syncs both About metadata versions from that owner before packaging.
Packaging rejects metadata that differs from the version owner.

Run `env -u RIMWORLD_MOD_DIR ./scripts/build-quiet.sh -c Release`.
The command builds .NET 10 / AnyCPU, runs contract checks, stages the complete mod and verifies its ZIP.
Successful runs print only `ok`. Each run keeps a full log under `.builds/`.
`.builds/build.log` links to the latest completed run.
The staged mod is `.builds/package/SimonSays`. The ZIP is `.builds/SimonSays.zip`.

Set `RIMWORLD_MOD_DIR` to the game's 1.7 Mods directory when installation is authorized.
On macOS, use the outer `RimWorld/Mods` path, even when it is a symlink into `RimWorldMac.app`.
The legacy `.app/Mods` alias is rejected.
The same command then installs the package and ZIP and verifies their contents.
Installation requires a stopped game. The workflow never starts or stops RimWorld.
Direct `dotnet build` performs no installation or packaging.

An existing installation must contain the exact published legacy files.
Missing, changed or additional legacy files stop installation before any installed file changes.
Matching legacy files and directories keep their inodes and modification times.
Installation updates only `1.7/`, About metadata and root metadata/documentation files.
Contract checks use disposable directories. They do not install into a real game.

# Published legacy baseline

`published-baseline.json` pins the installed public Workshop 1.2.1 payload by SHA-256.
It covers both old DLL sets and root `Defs/`, `Sounds/` and `Textures/`.
The repository's `Defs/Sounds.xml` and `Sounds/Simon-0001.wav` differ from that payload.
Their separate hashes detect accidental changes to the frozen repository files.

Packaging copies the published legacy bytes into staging.
It adds the current About metadata, load folders and 1.7 payload.
The repository variants of the changed resources live under `1.7/`.
For 1.7, load-folder precedence selects those variants over the published root resources.
Old versions continue to use the published root resources.

Set `SIMONSAYS_PUBLISHED_BASELINE` to a directory containing the pinned published payload.
For Workshop item `3044924185`, this is its directory under `steamapps/workshop/content/294100/`.
If the variable is unset, the workflow uses the ignored `.builds/published-baseline` cache.
All pinned files must match. Missing or changed baseline files stop the workflow before compilation or installation.
The workflow never substitutes differing repository files for the published legacy bytes.
