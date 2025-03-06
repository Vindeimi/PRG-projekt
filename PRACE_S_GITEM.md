# Práce s Gitem

## 📝 Úvod
Git je systém pro správu verzí, který umožňuje sledovat změny v kódu, spolupracovat s ostatními a spravovat různé verze projektu. Tento dokument popisuje základní příkazy a workflow pro práci s Gitem.

---

## 🔄 Základní workflow pro práci na vlastním úkolu

### 1. **Přepnutí na svou větev**
Nejdříve se ujistěte, že pracujete na své vlastní větvi. Můžete použít následující příkaz:
- `git checkout sam`  # Přepnutí na svou větev (nahraďte 'sam' svým jménem)

### 2. **Aktualizace z hlavní větve**
Předtím, než začnete pracovat na nových změnách, stáhněte nejnovější změny z hlavní větve `master`:
- `git pull origin master`  # Stažení nejnovější verze hlavní větve

### 3. **Provádění změn**
Nyní můžete začít pracovat na svých změnách. Jakmile dokončíte úpravy, přidejte soubory do stage:
- `git add .`  # Přidání všech změn do stage

### 4. **Commitování změn**
Po přidání změn do stage je čas vytvořit commit. Ujistěte se, že commit zpráva odpovídá konvencím:
- `git commit -m "[Feature] Přidána nová funkce"`  # Přizpůsobte zprávu svým změnám

### 5. **Odeslání změn na GitHub**
Jakmile máte hotové commity, můžete je odeslat na svůj repozitář na GitHub:
- `git push origin sam`  # Odeslání změn na svou větev

### 6. **Vytvoření Pull Requestu**
Po odeslání změn na GitHub vytvořte Pull Request (PR) do hlavní větve `master`. Otevřete repozitář na GitHubu a klikněte na tlačítko "Compare & pull request".

### 7. **Code review a schválení**
Po vytvoření PR počkejte na schválení od ostatních členů týmu. Všichni by měli zkontrolovat změny a poskytnout případnou zpětnou vazbu.

### 8. **Merge do `master`**
Jakmile je PR schváleno, můžete provést merge do hlavní větve `master`. To obvykle provádí osoba, která PR schválila.

### 9. **Údržba**
Po merge do `master` je dobré aktualizovat svou větev, abyste měli nejnovější změny. Použijte:
- `git checkout sam`  # Přepnutí na svou větev
- `git pull origin master`  # Aktualizace z hlavní větve

---
## 🚀 Commit zprávy  
- **Stručné a výstižné** (max 72 znaků v první větě).  
- Dodržujte jednotné prefixy commitů.  

> [!IMPORTANT]  
> Každý commit by měl odpovídat **jedné konkrétní změně** – neslučujte více různých úprav do jednoho commitu!  

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
```

---

## 📚 Základní příkazy Gitu

- **`git status`** – Zobrazí aktuální stav vaší pracovní kopie (změny, které byly provedeny).
- **`git log`** – Zobrazí historii commitů.
- **`git branch`** – Zobrazí seznam větví v repozitáři.
- **`git checkout -b nová_větev`** – Vytvoří novou větev a přepne se na ni.
- **`git merge název_větve`** – Sloučí změny z vybrané větve do aktuální větve.
- **`git pull origin jméno_větve`** – Stáhne a sloučí změny z vybrané větve na GitHubu do vaší aktuální větve.

---

> [!CAUTION]
> ## Pozor na příkazy, které můžou vytvořit konflikty.
> ```sh
> git push --force #Přepíše historii na vzdáleném repozitáři. Může vést ke ztrátě dat
> ```
> ```sh
> git reset --hard #Zruší všechny neukládané změny. Může způsobit ztrátu dat.
> ```
>  ```sh
> git rebase #Mění historii. Může vyžadovat --force při push.
> ```
>  ```sh
> git clean -f #Odstraní neukládané soubory. Zkontrolujte, co bude odstraněno.
> ```
> ```sh
> git cherry-pick #Aplikuje jednotlivé commity z jiné větve. Může způsobit konflikty.
> ```
> ```sh
> git push --delete #Odstraní vzdálenou větev. Buďte opatrní.
> ```
>  ```sh
> git merge --abort #Zruší merge. Ujistěte se, že jste uložili důležité změny.
> ```

---

## ⚠️ Pozor na konflikty

Při sloučení větví může dojít ke konfliktům, pokud byly ve stejných místech kódu provedeny různé změny. V takovém případě Git označí konfliktní soubory a budete muset ručně vyřešit konflikty před dokončením merge.

---

# Workflow

## 🛠️ Nastavení repozitáře

### 1️⃣ Klonování existujícího repozitáře
Pokud chceš pracovat na projektu, který už je na GitHubu:
```sh
# Naklonuješ si projekt do složky
git clone https://github.com/UZIVATEL/NAZEV-REPO.git
cd NAZEV-REPO
```

### 2️⃣ Inicializace nového repozitáře
Pokud chceš začít nový projekt:
```sh
# Spustí git v aktuální složce
git init
```
Pak přidej vzdálený repozitář (pokud existuje):
```sh
git remote add origin https://github.com/UZIVATEL/NAZEV-REPO.git
```

## 🚀 Přidávání a commitování změn

### 3️⃣ Přidání souborů ke commitu
```sh
# Přidání konkrétního souboru
git add soubor.txt

# Přidání všech souborů
git add .
```

### 4️⃣ Commit změn
```sh
# Používej správný formát commit message
# [BUILD] – změna struktury projektu
# [FEAT] – přidání nové funkce/souboru
# [FIX] – oprava chyby
# [DOCS] – změna v dokumentaci

git commit -m "[BUILD] Přidána nová složka pro assets"
```

## 🔄 Synchronizace s GitHubem

### 5️⃣ Stažení změn před pushnutím (pull)
```sh
# Stáhne změny z GitHubu a sloučí s lokálním repozitářem
git pull origin master --rebase
```

### 6️⃣ Odeslání změn na GitHub (push)
```sh
# Odesílá změny do hlavní větve
git push origin master
```

## ❌ Řešení problémů

### ❗ 7️⃣ Nelze pushnout ("non-fast-forward" error)
```sh
# Nejprve stáhni změny z GitHubu
git pull origin master --rebase
# --rebase je alternativa k merge -> nedochází ke commitu navíc a je přehlednější

# Pak zkus znovu pushnout
git push origin master
```

### ❗ 8️⃣ Merge konflikt
Pokud máš konflikt mezi verzemi souboru:
```sh
# Otevři soubor, oprav konflikt ručně
# Pak ho přidej do commitu a potvrď

git add soubor.txt
git commit -m "[FIX] Opraven merge konflikt"
git push origin master
```

### ❗ 9️⃣ "fatal: refusing to merge unrelated histories"
Pokud Git hlásí, že historie není propojená:
```sh
git pull origin master --allow-unrelated-histories
```

## 🗑️ Mazání souborů/složek z repozitáře

### 🔟 Smazání souboru a jeho odstranění z repozitáře
```sh
rm soubor.txt
git add soubor.txt
git commit -m "[BUILD] Odstraněn nepotřebný soubor"
git push origin master
```

### 🔟 Smazání složky
```sh
rm -rf slozka/
git add -A
git commit -m "[BUILD] Smazána složka s nepotřebnými soubory"
git push origin master
```

---
💡 **Tip:** Pokud něco nefunguje, podívej se na stav Git repozitáře:
```sh
git status
```
---
