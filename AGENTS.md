# Project workflow

- Run `env -u RIMWORLD_MOD_DIR ./scripts/build-quiet.sh -c Release` for local build, contract checks, package staging and ZIP verification.
- The staged mod is `.builds/package/SimonSays`. The ZIP is `.builds/SimonSays.zip`.
- Set `RIMWORLD_MOD_DIR` to a RimWorld 1.7 Mods directory when installation is authorized.
- Installation refuses a running game and mismatched installed legacy files. Matching legacy files remain in place.
- The workflow never starts or stops RimWorld. Direct `dotnet build` compiles only.
- Read `scripts/README.md` for the required published baseline and its provenance.
- Preserve `1.5/`, `1.6/`, root `Defs/`, `Sounds/` and `Textures/`. Put current builds and resource changes under `1.7/`.
- Stage old versions and root resources from the hash-pinned published baseline. Never substitute differing repository resources.

- `Directory.Build.props` owns the mod version. Bump it for a port.
- Every normal build syncs `About/About.xml` and `About/Manifest.xml` from that version owner.
- Installation must preserve matching frozen payload files in place, including their inodes and modification times.
- If an existing installation has missing, changed or additional frozen payload files, fail before changing installed files.
