# Bet or Die — Project Handoff for Claude

## Overview
Unity gambling/survival game. The player faces demons and chooses to bet or refuse each encounter, spending resources to escape.

## Core Concept
- **Title:** Bet or Die
- **Genre:** Gambling / Survival
- **Engine:** Unity (1人称 3D)
- **Status:** Pre-production（コア仕様確定済み、実装未着手）
- **Repository:** GitHub (shared across two PCs — always pull before working)

---

## Resource System

| リソース | 初期値 | 単位価値 |
|---------|--------|--------|
| 金 | 500G | 1G |
| 寿命 | 80year | 1year = 100G |
| 四肢 | 5本 | 1本 = 1000G |

- 四肢の内訳：右腕・右足・左腕・左足・頭
- 変換方向：四肢→寿命→金のみ可（逆は一部イベントのみ）
- 変換タイミング：任意（複雑になるなら端数切り上げに切り替え）
- 寿命・四肢は初期値が上限（ギャンブルで勝っても超えない）、金は上限なし

### 四肢の制約
- 両腕喪失 → カード系ギャンブル不可
- 両足喪失 → レース系ギャンブル不可
- 片腕・片足のみ喪失 → ペナルティなし

---

## Gameplay Loop

画面遷移：スタート → 説明 → ゲーム画面

- ターン制、1人称3D、部屋を1つずつ進む
- 各部屋で悪魔が「BET or DIE?」＋事前倍率を提示
- プレイヤーはBETかDIEを選択

---

## BET

- 悪魔がランダムで「金」か「寿命」のどちらを賭けるか指定
  - 寿命指定の確率は低め、ステージ進行で上昇
- ベット額：最低額は強制（ステージ進行で上昇）、上限内でプレイヤーが任意設定
- 勝ち：ベット額 × 事前倍率 × ゲーム内結果倍率 を獲得
  - 上限超過分は下位リソースに変換（寿命→金、四肢→寿命→金）
- 負け：ベット額のみ失う（倍率乗算なし）
- 金が尽きてベット時：警告ダイアログ（Yes/No）で任意変換

---

## DIE

- 寿命10yearを消費して次の部屋へ進む（暫定値、レベルデザインで調整）
- ステージ進行でDIE消費量も増加
- 寿命0のとき選択 → 悪魔がランダムで四肢を1本奪う
- 頭のみ残っている状態でDIE → 即ゲームオーバー
- インゲームで寿命が0になった場合はステージクリアまで猶予あり
  - クリア前に寿命を回復できればゲームオーバー回避可能

---

## ゲームオーバー条件

- 四肢（頭含む）をすべて失う
- 頭を賭けて負ける
- DIEで頭しか残っていない状態で選択
- ステージクリア時に寿命が0

---

## 倍率システム

### 事前倍率
- BET or DIE選択画面で表示（意思決定の判断材料）
- 悪魔の性格・ステージ・ギャンブル種目によって変動

| ステージ | 事前倍率の範囲 |
|----------|--------------|
| Stage 1〜2 | ×1.1 〜 ×1.5 |
| Stage 3〜5 | ×1.5 〜 ×3.0 |
| Stage 6〜  | ×2.0 〜 ×10.0 |

### 倍率と勝率の連動
| 事前倍率 | 勝率補正 |
|----------|----------|
| ×1.0〜×1.5 | 補正なし |
| ×1.5〜×3.0 | -5% |
| ×3.0〜×6.0 | -10% |
| ×6.0〜    | -15% |

### 計算式
- 勝ち：ベット額 × 事前倍率 × ゲーム内結果倍率
- 負け：ベット額のみ失う

---

## ギャンブル種目（7種）

| 種目 | 分類 | 備考 |
|------|------|------|
| ブラックジャック | カード系 | 両腕喪失で不可 |
| バカラ | カード系 | 両腕喪失で不可 |
| チンチロ | ダイス系 | Unity物理エンジン使用 |
| 丁半 | ダイス系 | Unity物理エンジン使用 |
| 動物レース | レース系 | 両足喪失で不可 |
| ルーレット | その他 | 0〜36 |
| スロット | その他 | 3リール |

### 各種目のゲーム内倍率

**ブラックジャック**
- 通常勝ち：×1 / ブラックジャック（2枚で21）：×1.5
- 悪魔は16以下ヒット強制、17以上スタンド強制

**バカラ**
- プレイヤー勝ち：×2 / バンカー勝ち：×2 / タイ：×8
- コミッション（手数料）なし

**チンチロ**
| 役 | 条件 | 倍率 |
|----|------|------|
| ピンゾロ | 1-1-1 | ×5 |
| シゴロ | 4-5-6 | ×3 |
| ゾロ目 | 同じ目3つ（ピンゾロ除く） | ×2 |
| 目あり | ペア＋別の目 | ×1 |
| ヒフミ | 1-2-3 | 負け |
| 目なし | 役なし | 負け |

**丁半**
- 丁（偶数）か半（奇数）を予想：×2
- ゾロ目が出た場合：悪魔総取り（的中でも没収）

**動物レース**
- 4体の使い魔がレース、単勝のみ
- オッズ例：×1.5 / ×2 / ×3 / ×6

**ルーレット**
| 賭け方 | 倍率 |
|--------|------|
| 数字1点 | ×35 |
| 赤/黒 | ×2 |
| 奇数/偶数 | ×2 |
| 1〜18/19〜36 | ×2 |
- 0が出た場合：悪魔総取り

**スロット**
| 結果 | 倍率 |
|------|------|
| 悪魔3つ揃い | ×10 |
| その他3つ揃い | ×3 |
| 2つ揃い | ×1.5 |
| 揃いなし | 負け |

---

## 悪魔（4種）

| 名前 | 性格 | リスク傾向 | 担当ギャンブル | 倍率選択 |
|------|------|-----------|--------------|--------|
| アモン | 好戦的 | ハイリスクハイリターン | バカラ・ルーレット | レンジの上半分から抽選 |
| ストラス | 臆病 | ローリスクローリターン | 丁半・ブラックジャック | レンジの下半分から抽選 |
| マモン | 勝負師 | 金=ミドル／寿命=ハイリスクハイリターン | 動物レース・チンチロ | 金ベット時はレンジ中央、寿命強制時は最低額が跳ね上がりリターンも増大 |
| ベルフェゴール | 怠け者 | ローリスクミドルリターン | スロット・動物レース | レンジの下〜中央から抽選 |

---

## ステージ構造（ローグライク）

- 部屋数：8 / 12 / 16 / 20 からランダム
  - 少ない部屋数 → 最低ベット額が高い・DIE消費が重い
- クリア条件：Stage1=2000G、以降ステージごとに倍（4000G→8000G→…）
- イベント部屋：約4部屋ごとに確率抽選
  - 確率：15%→30%→45%→60%→70%（上限）、発生時にリセット
  - 内容：回復 or 恒久スキル獲得（詳細未定）
  - スキルはクリアまたはゲームオーバーまで継続

---

## エンディング

- ステージ上限なし（エンドレス）
- 3ステージクリア後、各ステージクリアごとに「逃げる」選択肢が出現
  - 逃げる → 正規クリア、獲得金額表示で現世に帰還
  - 続行 → 次ステージへ
- 7〜8ステージ到達 → 悪魔が人間を追放する特殊演出エンド
- 3ステージ以降でゲームオーバー → 部分クリアなし、純粋なゲームオーバー
- Stage1〜2で逃げようとすると悪魔に捕まり強制続行（演出）

---

## 実装方針

- 全パラメータは **ScriptableObject** で管理、UnityのInspectorから変更可能
- データファイル構成例：
  - `GameConfig`（全体設定・リソース初期値）
  - `StageConfig`（ステージごとの倍率範囲・最低ベット額・DIE消費量）
  - `DemonData`（悪魔ごとの設定）
  - `BlackjackData` / `RouletteData` 等（各ギャンブルの数値）

---

## 寿命ベット確率
- 初期：20%、ステージごとに+3〜4%、上限：40%（ScriptableObjectで変更可能）

## スキル一覧

| カテゴリ | スキル | 強度 | 入手先 |
|---------|--------|------|--------|
| ギャンブル強化 | 特定ギャンブルの勝率+X% | 弱〜中 | ショップ（通常） |
| ギャンブル強化 | ゲーム内倍率が低い結果を1回再ロール | 中 | ショップ（高額）・スキル付与 |
| ギャンブル強化 | ブラックジャックで悪魔の伏せカードを1枚見る | 中 | ショップ・スキル付与 |
| リソース管理 | DIEの寿命消費を軽減（永続） | 中 | ショップ（高額）・スキル付与 |
| リソース管理 | 寿命ベット時の最低額を下げる | 弱〜中 | ショップ（通常） |
| リソース管理 | リソース変換レートを改善 | 弱 | ショップ（通常） |
| 防御・保険 | 賭けに負けても1回リソースを失わない | 強 | スキル付与・囚われた人間 |
| 防御・保険 | DIEで失う四肢を自分で選択できる | 強 | スキル付与・囚われた人間 |
| 防御・保険 | ゲームオーバー条件を1回無効化 | 超強 | 囚われた人間（レア） |
| 情報 | 次の部屋を3択から選べる（悪魔・倍率・通常orイベントが見える） | 強 | スキル付与・囚われた人間 |
| 呪いセット | 強欲の呪い（金獲得+50% / 寿命ベット確率+20%） | 強＋デメリット | 呪い部屋 |
| 呪いセット | 臆病の呪い（負けても寿命失わない / 倍率常に最低値） | 強＋デメリット | 呪い部屋 |
| 呪いセット | 悪魔の加護（四肢ゲームオーバー1回回避 / 毎部屋1year消費） | 強＋デメリット | 呪い部屋 |

※ショップスキルは基本的にイベント部屋スキルより弱め、高額商品として同等強度のものも存在

---

## Workflow
- Unity project, version controlled via GitHub
- Two development machines share the same repo
- **Always `git pull` before starting a session**
- Prefer small, focused commits

## For Claude on a New Machine
1. Read this file first — full spec is written here
2. Check `git log --oneline -10` to see recent changes
3. Ask the user what they want to work on today
4. Do not assume any code exists — verify with file search before referencing implementation details

---

## UI配置設計（2026-06-16確定）

### Canvas設定
- Render Mode: Screen Space - Overlay
- UIManager.cs をCanvasにアタッチ、全パネルの参照をInspectorからアサイン

### パネル一覧（UIManager [Panels] セクション順）

| # | フィールド名 | サイズ／位置 | 初期状態 |
|---|-------------|-------------|---------|
| 1 | startScreenPanel | フルスクリーン（Stretch） | Active |
| 2 | explanationPanel | フルスクリーン（Stretch） | Inactive（中身未実装・将来ハイライト型チュートリアル予定） |
| 3 | betOrDiePanel | 中央 W:360 H:280 | Inactive |
| 4 | bettingPanel | 中央 W:380 H:220 | Inactive |
| 5 | gamblingPanel | フルスクリーン（Stretch） | Inactive |
| 6 | eventRoomPanel | 中央 W:400 H:320 | Inactive |
| 7 | stageEndPanel | 中央 W:360 H:260 | Inactive |
| 8 | gameOverPanel | フルスクリーン（Stretch） | Inactive |
| 9 | victoryPanel | フルスクリーン（Stretch） | Inactive |
| 10 | demonBanishmentPanel | フルスクリーン（Stretch） | Inactive |
| 11 | goldDepletionDialogPanel | 中央 W:300 H:160 | Inactive（ShowGoldDepletionDialog()で制御） |

### HUD（UIManager [HUD] セクション）

| フィールド名 | コンポーネント | 位置 | 子要素 |
|-------------|--------------|------|--------|
| resourceUI | ResourceUI.cs | 左上固定（Anchor: top-left） W:140 H:100 | TMP×4（goldText / lifespanText / stageText / roomText） |
| limbUI | LimbUI.cs | 右中央固定（Anchor: middle-right） W:70 H:140 | Image×5（RightArm / LeftArm / RightLeg / LeftLeg / Head）欠損時に暗転 |

### 各パネルの主要子要素

**1. startScreenPanel**
- TextMeshProUGUI: タイトル「Bet or Die」（中央上）
- TextMeshProUGUI: サブタイトル（中央）
- Button: スタート → SceneController.GoToGame()（中央下）

**2. explanationPanel** — 中身未実装、GameObjectのみ配置

**3. betOrDiePanel**
- TextMeshProUGUI: 悪魔名（中央）← UIで表示するのはこれのみ
- ※BET/DIE選択肢は3Dプレハブで実装（UI構築完了後に着手予定）

**4. bettingPanel**
- TextMeshProUGUI: 通貨＋事前倍率ラベル（上部）
- TextMeshProUGUI: 現在のベット額（中央大）
- Slider: ベット額調整（min〜max）
- TMP_InputField: 手入力用（任意）
- Button: 確定 → BetController.ConfirmBet()（下部）

**5. gamblingPanel**
- GamblingUI.cs をこのパネルにアタッチ
- 子Panel×7（デフォルト全Inactive、GamblingUI.ShowGame()で切替）:
  BlackjackPanel / BaccaratPanel / ChinchiroPanel / ChoHanPanel / AnimalRacePanel / RoulettePanel / SlotsPanel

**6. eventRoomPanel**
- Image: イベントアイコン（中央上）
- TextMeshProUGUI: イベント名（中央）
- TextMeshProUGUI: イベント説明（中央）
- Button: アクション（イベント種別で動的テキスト変更） → EventRoomController
- Button: スキップ/閉じる

**7. stageEndPanel**
- TextMeshProUGUI: 「Stage X Clear!」バッジ（上部）
- TextMeshProUGUI: 現在の金額（中央）
- TextMeshProUGUI: 次ステージ目標金額（中央）
- Button: 逃げる → EndingController.TriggerEscape()（左下、Stage3以降のみ表示）
- Button: 続行 → GameManager.ChangeState()（右下）

**8. gameOverPanel**
- TextMeshProUGUI: 「Game Over」（赤、中央上）
- TextMeshProUGUI: 獲得金額（中央）
- Button: タイトルへ → SceneController.GoToStart()（中央下）

**9. victoryPanel**
- TextMeshProUGUI: 「Escape!」バッジ（上部）
- TextMeshProUGUI: 稼いだ金額（中央）
- TextMeshProUGUI: クリアステージ数（中央）
- Button: タイトルへ → SceneController.GoToStart()（中央下）

**10. demonBanishmentPanel**
- Image: 悪魔ポートレート大（中央上）
- TextMeshProUGUI: 悪魔の台詞（イタリック）
- Button: タイトルへ → SceneController.GoToStart()（中央下）

**11. goldDepletionDialogPanel**
- TextMeshProUGUI: 警告テキスト（黄色）
- TextMeshProUGUI: 変換内容説明
- Button: Yes → BetController.ConfirmConversionAndBet()（左）
- Button: No → UIManager.HideGoldDepletionDialog()（右）

---

## 実装進捗ログ

### 2026-06-13 — 仕様確定 & 全スクリプト実装完了

**完了したこと**
- コア仕様すべて確定（リソース・BET/DIE・倍率・ステージ・ギャンブル7種・悪魔4体・スキル13種・イベント部屋6種・エンディング）
- 全46スクリプト・ScriptableObjectを実装しGitHubにコミット済み
- コンパイルエラー3件修正済み
  - `DiceController.cs`: `linearVelocity` → `velocity`（Unity旧バージョン対応）
  - `GamblingManager.cs`: 未使用変数 `insuranceUsed` を削除
  - `ResourceManager.cs`: 未使用イベント `OnGoldDepletionWarning` を削除
- UIManager 全13フィールドの内部構造・子要素・アンカー配置を設計済み

**実装済みスクリプト一覧**
```
Assets/Scripts/
  Data/         GameEnums, GameConfig, StageConfig, DemonData, SkillData
                BlackjackData, BaccaratData, ChinchiroData, ChoHanData,
                AnimalRaceData, RouletteData, SlotData, GamblingGameData
  Core/         GameManager, ResourceManager, StageManager
  Room/         RoomController, DemonController, DieController, BetController
  Gambling/     GamblingManager, DiceController,
                BlackjackGame, BaccaratGame, ChinchiroGame, ChoHanGame,
                AnimalRaceGame, RouletteGame, SlotGame
  Skills/       SkillManager
  Events/       EventRoomController
  Endings/      EndingController
  UI/           UIManager, GamblingUI, ResourceUI, LimbUI, BetUI
  SceneController
```

**次のステップ（未着手）**
1. UnityエディタでSceneを構築（Canvas / UIオブジェクト作成 → UIManagerにアサイン）
2. ScriptableObjectインスタンスを `Assets/Data/` に作成し数値を入力
3. 基本フローの動作確認（StartScreen → BetOrDie → Gambling → StageEnd）
4. 各ギャンブルゲームのUI実装（カード・サイコロ・スロット等）

**未着手・保留中**
- ExplanationPanel: パネルのGameObjectは作成するが中身は未実装。将来的にハイライト型チュートリアル（ゲーム画面上に矢印・フキダシを重ねて順番に操作説明するタイプ）を実装予定。
- BetOrDiePanel の3D化（UIの構築完了後に着手）:
  - BetOrDiePanel は悪魔名TMPのみ残す
  - BET/DIE選択肢を3Dプレハブに変更（BetChoice×7種 / DieChoice×2種）
  - 悪魔の手の上にスポーンしクリックで選択
  - 新規: ChoiceObject.cs / ChoicePrefabRegistry.cs
  - 変更: DemonController / BetController / DieController
