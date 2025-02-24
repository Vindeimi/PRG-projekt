# Pravidla přispívání do projektu

## 🔄 Práce s větvemi
- Hlavní větev je `master`, každý má svou vlastní větev (`sam`, `honza`, `pavel`).
- Po dokončení změn se dělá **Pull Request (PR)** do `master`.

## ✅ Code review
- PR musí být schválen alespoň jedním členem týmu.
- Kód musí být přehledný a odpovídat stylu projektu.

## 📏 Styl kódu
- Používáme vlastní **konvence názvů**:
  - **Public globální proměnná** → `int BigNum`
  - **Private globální proměnná** → `_bigNum`
  - **Lokální proměnná** → `bigNum`
  - **Funkce** → `BigNum()`
  - **Konstanty** → `BIG_NUM`
- Každá třída a metoda musí mít komentář (`///`).
- Konzistentní struktura složek:
  - **/Models** → Datové modely
  - **/Views** → WPF XAML soubory
  - **/ViewModels** → Logika aplikace (MVVM)
  - **/Services** → API komunikace

## 🚀 Commit zprávy
- **Stručné a výstižné** (max 72 znaků v první větě).
- **Prefixy**:
  -git commit -m "[Feature] Přidána kalkulace průměrné známky"
  -git commit -m "[Fix] Opraven bug ve výpočtu absence"
  -git commit -m "[Refactor] Přesunutí metody do jiné třídy"
  -git commit -m "[Docs] Přidána pravidla pro Git workflow"
  -git commit -m "[Style] Opraveny mezery a nesprávné názvy proměnných"
  -git commit -m "[Test] Přidán test pro API komunikaci"
  -git commit -m "[Chore] Aktualizace NuGet balíčků"
  -git commit -m "[Perf] Optimalizována načítací doba seznamu"
  -git commit -m "[Revert] Zrušení posledního PR kvůli chybám"

