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
- **Prefixy**
