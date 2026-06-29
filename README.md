# ExtraLib

Extra Lib is a crucial dependency mod that is indispensable for the operation of any tools within the EXTRA mod suite. It ensures seamless integration and enhanced functionality across the entire mod collection.

Special thanks to YenYan and T.D.W for their invaluable tips and assistance with coding and is help with the UI, as well as to CityRat for their exceptional contributions to thumbnails and all related graphic design work.

## Adding Assets to the EAM UI

You can now register any prefab into the **EAM** menu by adding a single component to your prefab.

### Setup

1. Add the **`EditorAssetCategoryOverride`** component to your prefab.
2. In the **`m_IncludeCategories`** field (string array), add an entry starting with `EAM`.

### Format
`EAM/<Category>/<Subcategory>/.../<FinalCategory>`
- Each `/` creates a **nested category level** in the UI menu.
- You need **at least 3 segments**: `EAM`, a root category, and a final category.
- The **last segment** can include an optional **priority** using `:` → `CategoryName:priority`
  - Priority is an integer that controls the **sort order** of the asset within its category (lower = appears first).
  - If omitted, defaults to `int.MaxValue (2147483647)`.

### Examples

`EAM/Surfaces/Ground`
→ Adds the asset under **Surfaces → Ground**, priority `int.MaxValue (2147483647)`

`EAM/Surfaces/Ground:2558`
→ Same location, but with priority `2558`

`EAM/Trees/Deciduous/Oak`
→ Creates a nested path: **Trees → Deciduous → Oak**, priority `int.MaxValue (2147483647)`

`EAM/Props/Urban/Benches:10`
→ **Props → Urban → Benches**, priority `10`

`EAM/Fences/Wooden/Panels:100`
→ **Fences → Wooden → Panels**, priority `100`

### Notes

- Only the **first valid** `EAM` entry is used (the loop breaks after processing one).
- If the prefab doesn't already have a `UIObject` component, one will be **created automatically** with a default icon.
- If it already has one, only the **priority** will be updated.
- Categories are **created on the fly** if they don't exist yet — no need to pre-register them.

# Translation
⚠️ This mod uses crowdin so there maybe inconsistencies with translations due to translations from different translators ⚠️
- ✔️French 100% : aricoseco
- ✔️Chinese Simplified 100% : RilkeXS(无手文) (RilkeXS), 一叶杳舟 (Yao_Zhou)
- ❌Chinese Traditional 0%
- ✔️German 100% : Konsi
- ❌Italian 0%
- ❌Japanese 0%
- ❌Korean 0%
- ✔️Polish 100% : Lisek
- ❌Portuguese, Brazilian 0%
- ❌Russian 0%
- ✔️Spanish 100% : elGendo87

Thanks krzychu124 for that.
Note to myself : https://dndkit.com/