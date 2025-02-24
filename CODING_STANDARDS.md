# 📜 Pravidla přispívání do projektu  

## 🔄 Práce s větvemi  
- Hlavní větev je `master`, každý má svou vlastní větev (`sam`, `honza`, `pavel`).  
- Po dokončení změn se dělá **Pull Request (PR)** do `master`.  

⚠️ **[WARNING]** Nepřidávejte nové změny přímo do `master` – vždy použijte svou větev a PR!  

💡 **[TIP]** Pokud pracujete na větší změně, používejte menší commity a pravidelně pushujte.  

---

## ✅ Code review  
- PR musí být schválen alespoň **jedním členem týmu**.  
- Kód musí být přehledný a odpovídat stylu projektu.  

📌 **[IMPORTANT]** Před odesláním PR si zkontrolujte:  
✔️ Kód odpovídá našemu stylu  
✔️ Změny jsou dobře zdokumentované  
✔️ Projekt se správně sestaví  

👀 **[TIP]** Pokud je PR větší, popište hlavní změny v popisu PR, ať se v něm ostatní rychle zorientují.  

---

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

⚠️ **[WARNING]** Nepoužívejte hardcoded hodnoty – používejte **konstanty** nebo **konfigurační soubory**!  

💡 **[TIP]** Pokud si nejste jisti, jak napsat kód správně, inspirujte se existujícím kódem v repozitáři.  

---

## 🚀 Commit zprávy  
- **Stručné a výstižné** (max 72 znaků v první větě).  
- Dodržujte jednotné prefixy commitů.  

📌 **[IMPORTANT]** Každý commit by měl odpovídat **jedné konkrétní změně** – neslučujte více různých úprav do jednoho commitu!  

### 🚀 Nejčastější typy commit zpráv:  
- **`[Feature]`** – Přidání nové funkce  
- **`[Fix]`** – Oprava chyby nebo bugu  
- **`[Refactor]`** – Změna kódu bez změny funkcionality  
- **`[Docs]`** – Úprava dokumentace (`README.md`, `CONTRIBUTING.md` atd.)  
- **`[Style]`** – Oprava formátování, mezery, lomítek – **žádná logická změna**  
- **`[Test]`** – Přidání nebo úprava testů  
- **`[Chore]`** – Údržba projektu (např. aktualizace knihoven)  
- **`[Build]`** – Změny ve **sestavení projektu**, jako konfigurace nebo CI/CD  
- **`[Perf]`** – Optimalizace výkonu  
- **`[Revert]`** – Vrácení předchozí změny  
- **`[TIPS]`** – Přidání užitečné rady do dokumentace  
- **`[WARNING]`** – Oprava kritické chyby nebo bezpečnostního problému  
- **`[IMPORTANT]`** – Důležitá změna v architektuře nebo klíčové opravě  

---

### 📌 **Příklady použití:**  
```bash
git commit -m "[Feature] Přidána kalkulace průměrné známky"
git commit -m "[Fix] Opraven bug ve výpočtu absence"
git commit -m "[Refactor] Přesunutí metody do jiné třídy"
git commit -m "[Docs] Přidána pravidla pro Git workflow"
git commit -m "[Style] Opraveny mezery a nesprávné názvy proměnných"
git commit -m "[Test] Přidán test pro API komunikaci"
git commit -m "[Chore] Aktualizace NuGet balíčků"
git commit -m "[Perf] Optimalizována načítací doba seznamu"
git commit -m "[Revert] Zrušení posledního PR kvůli chybám"
