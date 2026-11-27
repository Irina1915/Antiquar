using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using AntiqueShopLibrary;
using AntiqueShopLibrary.BD;
using Antique_shop;

namespace WinFormsApp2
{
    public partial class MainForm : Form
    {
        // Изображения из каталога хранятся рядом с приложением в папке images.
        private static readonly Dictionary<string, string> ProductImageFiles = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Пейзаж «Золотая осень»"] = "catalog_003.jpg",
            ["Карманные часы «Молния»"] = "catalog_004.jpg",
            ["Письменный стол"] = "catalog_005.jpg",
            ["Серебряный рубль 1898 года"] = "catalog_006.jpg",
            ["Фарфоровая статуэтка «Балерина»"] = "Вазы.jpg",
            ["Брошь с янтарём"] = "catalog_022.jpg",
            ["Настольная лампа"] = "catalog_007.jpg",
            ["Кулон с гранатом"] = "catalog_008.jpg",
            ["Комод из ореха"] = "catalog_009.jpg",
            ["Медная монета СССР"] = "catalog_010.jpg",
            ["Стул с резной спинкой"] = "catalog_011.jpg",
            ["Будильник с ключом"] = "Часы фарфоровые в стиле.jpg",
            ["Серебряные серьги"] = "catalog_013.jpg",
            ["Настенные часы «Весна»"] = "catalog_014.jpg",
            ["Натюрморт с грушами"] = "catalog_015.jpg",
            ["Этюд «Старый сад»"] = "catalog_016.jpg",
            ["Ваза «Летний вечер»"] = "Вазы.jpg",
            ["Чайная пара с цветочным узором"] = "catalog_018.jpg",
            ["Серебряная монета 1901 года"] = "catalog_019.jpg",
            ["Зеркало в резной раме"] = "catalog_020.jpg",
            ["Подсвечник латунный"] = "catalog_021.jpg",
            ["Браслет в стиле ар-деко"] = "catalog_022.jpg",
            ["Самовар с гравировкой"] = "catalog_023.jpg",
            ["Самовар «Тула»"] = "catalog_023.jpg",
            ["Настольные часы в корпусе из дерева"] = "catalog_014.jpg",
            ["Фарфоровая тарелка с пейзажем"] = "Вазы.jpg"
        };

        private readonly UserService _userService;
        // Репозиторий каталога и отдельный репозиторий заявок используют одну базу.
        private readonly DBConnection _dbConnection;
        private readonly IProposedProductRepository _proposedProductRepository;
        private DataGridView _proposalsGrid;
        private ProposedProductService _proposedProductService;
        private CustomerRequestService _customerRequestService;
        private PurchaseHistoryService _purchaseHistoryService;
        private DataGridView _myRequestsGrid;
        private DataGridView _myPurchasesGrid;
        private DataGridView _purchaseArchiveGrid;
        private TextBox _purchaseSearchBox;
        private List<PurchaseRecord> _loadedPurchases = new List<PurchaseRecord>();
        private List<Tovar> _allProducts = new List<Tovar>();
        private TextBox _productSearchBox;
        private TextBox _authorFilterBox;
        private TextBox _countryFilterBox;
        private ComboBox _categoryFilterBox;
        private ComboBox _eraFilterBox;
        private ComboBox _authenticityFilterBox;
        private NumericUpDown _minimumPriceFilter;
        private NumericUpDown _maximumPriceFilter;
        private bool _isResettingProductFilters;

        // --- Экземпляр корзины ---
        private readonly Cart _cart;

        public MainForm(UserService userService)
        {
            // Подготавливает форму, сервисы, корзину и первоначальные данные.
            InitializeComponent();
            // Отметка подлинности отображается, но пользователь не может её переключить.
            dataGridView1.Columns["AuthenticityColumn"].ReadOnly = true;
            _userService = userService;

            // Инициализация БД
            string connString = DatabaseConfig.ConnectionString;
            _dbConnection = new DBConnection(connString);
            _proposedProductRepository = new ProposedProductDBConnection(connString);
            button1.Click += ButtonProposeProduct_Click;

            // --- Инициализация корзины ---
            _cart = new Cart();

            LoadUserInfo();
            ConfigureProductFilters();
            _customerRequestService = new CustomerRequestService(_proposedProductRepository);
            _purchaseHistoryService = new PurchaseHistoryService(_dbConnection);
            LoadProducts();
            ConfigureProposalsTab();
            ConfigurePersonalTabs();

            // Настраиваем таблицу корзины при запуске
            SetupCartGrid();
        }

        private void ConfigureProductFilters()
        {
            // Освобождает верхнюю полосу вкладки «Товары» под поиск и фильтры.
            comboBox1.Visible = false;
            panel1.Location = new Point(6, 82);
            panel1.Size = new Size(1207, 490);
            panel1.BackColor = Color.White;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.PeachPuff;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            var toolbar = new FlowLayoutPanel
            {
                Location = new Point(6, 5),
                Size = new Size(1207, 73),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.PeachPuff,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            _productSearchBox = new TextBox { PlaceholderText = "Поиск по наименованию", Width = 185 };
            _categoryFilterBox = CreateFilterCombo("Все категории");
            _eraFilterBox = CreateFilterCombo("Все эпохи");
            _authorFilterBox = new TextBox { PlaceholderText = "Автор", Width = 125 };
            _countryFilterBox = new TextBox { PlaceholderText = "Страна", Width = 125 };
            _authenticityFilterBox = CreateFilterCombo("Любая подлинность", "Подлинный", "Не подтверждён");
            _minimumPriceFilter = new NumericUpDown { Minimum = 0, Maximum = 1000000000, Increment = 100, ThousandsSeparator = true, Width = 110 };
            _maximumPriceFilter = new NumericUpDown { Minimum = 0, Maximum = 1000000000, Value = 1000000000, Increment = 100, ThousandsSeparator = true, Width = 110 };

            toolbar.Controls.Add(CreateFilterGroup("Наименование", _productSearchBox, 180));
            toolbar.Controls.Add(CreateFilterGroup("Категория", _categoryFilterBox, 135));
            toolbar.Controls.Add(CreateFilterGroup("Эпоха", _eraFilterBox, 135));
            toolbar.Controls.Add(CreateFilterGroup("Автор", _authorFilterBox, 125));
            toolbar.Controls.Add(CreateFilterGroup("Страна", _countryFilterBox, 125));
            toolbar.Controls.Add(CreateFilterGroup("Подлинность", _authenticityFilterBox, 140));
            toolbar.Controls.Add(CreateFilterGroup("Цена от, руб.", _minimumPriceFilter, 115));
            toolbar.Controls.Add(CreateFilterGroup("Цена до, руб.", _maximumPriceFilter, 115));
            var resetFiltersButton = new Button
            {
                Text = "Сбросить", Width = 92, Height = 28,
                Margin = new Padding(6, 22, 1, 0),
                BackColor = Color.Sienna, ForeColor = Color.PeachPuff,
                FlatStyle = FlatStyle.Flat
            };
            resetFiltersButton.Click += ResetProductFilters;
            toolbar.Controls.Add(resetFiltersButton);
            tabPage2.Controls.Add(toolbar);
            toolbar.BringToFront();
            // Полностью перерисовывает каталог после переключения вкладки, чтобы старые
            // элементы главной страницы не оставались поверх строк таблицы.
            tabControl1.Selected += (s, e) =>
            {
                if (e.TabPage != tabPage2) return;
                tabPage2.Invalidate(true);
                panel1.Invalidate(true);
                dataGridView1.Invalidate(true);
                dataGridView1.Refresh();
            };

            _productSearchBox.TextChanged += ProductFilter_Changed;
            _categoryFilterBox.SelectedIndexChanged += ProductFilter_Changed;
            _eraFilterBox.SelectedIndexChanged += ProductFilter_Changed;
            _authorFilterBox.TextChanged += ProductFilter_Changed;
            _countryFilterBox.TextChanged += ProductFilter_Changed;
            _authenticityFilterBox.SelectedIndexChanged += ProductFilter_Changed;
            _minimumPriceFilter.ValueChanged += ProductFilter_Changed;
            _maximumPriceFilter.ValueChanged += ProductFilter_Changed;
        }

        private void ProductFilter_Changed(object sender, EventArgs e)
        {
            if (!_isResettingProductFilters)
                ApplyProductFilters();
        }

        private void ResetProductFilters(object sender, EventArgs e)
        {
            _isResettingProductFilters = true;
            try
            {
                _productSearchBox.Clear();
                _categoryFilterBox.SelectedIndex = 0;
                _eraFilterBox.SelectedIndex = 0;
                _authorFilterBox.Clear();
                _countryFilterBox.Clear();
                _authenticityFilterBox.SelectedIndex = 0;
                _minimumPriceFilter.Value = _minimumPriceFilter.Minimum;
                _maximumPriceFilter.Value = _maximumPriceFilter.Maximum;
            }
            finally
            {
                _isResettingProductFilters = false;
            }

            ApplyProductFilters();
        }

        private static ComboBox CreateFilterCombo(string firstItem, params string[] otherItems)
        {
            var combo = new ComboBox { Width = 135, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.Add(firstItem);
            combo.Items.AddRange(otherItems);
            combo.SelectedIndex = 0;
            return combo;
        }

        private static Panel CreateFilterGroup(string caption, Control input, int width)
        {
            var group = new Panel { Width = width, Height = 68, Margin = new Padding(1, 0, 1, 0) };
            group.Controls.Add(new Label
            {
                Text = caption,
                AutoSize = false,
                Location = new Point(0, 0),
                Size = new Size(width - 2, 20),
                ForeColor = Color.SaddleBrown
            });
            input.Location = new Point(0, 22);
            input.Height = 28;
            input.Width = width - 3;
            group.Controls.Add(input);
            return group;
        }

        private void PopulateFilterChoices()
        {
            SetFilterChoices(_categoryFilterBox, "Все категории", _allProducts.Select(p => p.Categoria));
            SetFilterChoices(_eraFilterBox, "Все эпохи", _allProducts.Select(p => p.Era));
        }

        private static void SetFilterChoices(ComboBox combo, string allText, IEnumerable<string> choices)
        {
            string selected = combo.SelectedItem?.ToString() ?? allText;
            combo.BeginUpdate();
            combo.Items.Clear();
            combo.Items.Add(allText);
            foreach (string choice in choices.Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.CurrentCultureIgnoreCase).OrderBy(value => value))
                combo.Items.Add(choice);
            combo.SelectedItem = combo.Items.Contains(selected) ? selected : allText;
            combo.EndUpdate();
        }

        private void ApplyProductFilters()
        {
            if (_productSearchBox == null) return;
            bool? authenticity = null;
            if (_authenticityFilterBox.SelectedItem?.ToString() == "Подлинный") authenticity = true;
            if (_authenticityFilterBox.SelectedItem?.ToString() == "Не подтверждён") authenticity = false;

            var criteria = new ProductFilterCriteria
            {
                SearchText = _productSearchBox.Text.Trim(),
                Category = _categoryFilterBox.SelectedIndex <= 0 ? string.Empty : _categoryFilterBox.Text,
                MinimumPrice = _minimumPriceFilter.Value > 0 ? (double?)_minimumPriceFilter.Value : null,
                MaximumPrice = _maximumPriceFilter.Value < _maximumPriceFilter.Maximum ? (double?)_maximumPriceFilter.Value : null,
                Era = _eraFilterBox.SelectedIndex <= 0 ? string.Empty : _eraFilterBox.Text,
                Author = _authorFilterBox.Text.Trim(),
                Country = _countryFilterBox.Text.Trim(),
                Authenticity = authenticity
            };
            ShowProducts(ProductCatalogFilter.Apply(_allProducts, criteria));
        }

        private void ShowProducts(IEnumerable<Tovar> products)
        {
            ClearGridImages(dataGridView1);
            dataGridView1.Rows.Clear();
            foreach (var prod in products)
            {
                Image photo = null;
                string photoPath = ResolveProductPhotoPath(prod.Foto, prod.Name);
                if (!string.IsNullOrEmpty(photoPath))
                {
                    try { photo = Image.FromFile(photoPath); } catch { }
                }
                int rowIndex = dataGridView1.Rows.Add(photo, prod.Name, prod.Categoria, prod.Era, prod.Prise,
                    prod.Autor, prod.Country, prod.Opisanie, prod.AuthenMark == "Да");
                dataGridView1.Rows[rowIndex].Height = 100;
                dataGridView1.Rows[rowIndex].Tag = prod;
            }
        }

        private void ConfigureProposalsTab()
        {
            // Вкладка и действия модерации доступны только администраторам.
            if (_userService.CurrentUser == null || _userService.CurrentUser.Role != Role.Admin)
            {
                tabControl1.TabPages.Remove(tabPage4);
                return;
            }
            tabPage4.Text = "Предложения";
            tabPage4.BackColor = Color.PeachPuff;
            _proposedProductService = new ProposedProductService(_proposedProductRepository, _userService.CurrentUser.Role);
            _proposalsGrid = new DataGridView
            {
                Location = new Point(8, 8), Size = new Size(1140, 555),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, RowTemplate = { Height = 90 }
            };
            _proposalsGrid.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Photo", HeaderText = "Фото", ImageLayout = DataGridViewImageCellLayout.Zoom, FillWeight = 75
            });
            _proposalsGrid.Columns.Add("Id", "№");
            _proposalsGrid.Columns.Add("Name", "Наименование");
            _proposalsGrid.Columns.Add("Category", "Категория");
            _proposalsGrid.Columns.Add("Era", "Эпоха");
            _proposalsGrid.Columns.Add("Price", "Цена");
            _proposalsGrid.Columns.Add("Author", "Автор");
            _proposalsGrid.Columns.Add("Country", "Страна");
            _proposalsGrid.Columns.Add("Description", "Описание");
            _proposalsGrid.Columns.Add("Phone", "Телефон");
            var actionsPanel = new Panel
            {
                Location = new Point(1158, 8), Size = new Size(240, 555),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.PeachPuff
            };
            var approve = CreateProposalButton("Одобрить", 12, 64);
            var processing = CreateProposalButton("На рассмотрение", 12, 136);
            var reject = CreateProposalButton("Отклонить", 12, 208);
            approve.Click += (s, e) => ProcessProposal("approve");
            processing.Click += (s, e) => ProcessProposal("processing");
            reject.Click += (s, e) => ProcessProposal("reject");
            actionsPanel.Controls.Add(approve);
            actionsPanel.Controls.Add(processing);
            actionsPanel.Controls.Add(reject);
            tabPage4.Controls.Add(_proposalsGrid);
            tabPage4.Controls.Add(actionsPanel);
            tabControl1.Selected += (s, e) => { if (e.TabPage == tabPage4) LoadProposals(); };
            LoadProposals();
        }

        private Button CreateProposalButton(string text, int x, int y) => new Button
        {
            Text = text, Location = new Point(x, y), Size = new Size(216, 56),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.Sienna, ForeColor = Color.PeachPuff, FlatStyle = FlatStyle.Flat
        };

        private void ConfigurePersonalTabs()
        {
            // Добавляет личные заявки и покупки; архив показывается только администратору.
            User user = _userService.CurrentUser;
            if (user == null || user.Id <= 0) return;

            if (user.Role != Role.Guest)
            {
                var requestsTab = new TabPage("Мои заявки") { BackColor = Color.PeachPuff };
                _myRequestsGrid = CreateReadOnlyGrid();
                AddTextColumn(_myRequestsGrid, "Name", "Наименование");
                AddTextColumn(_myRequestsGrid, "Category", "Категория");
                AddTextColumn(_myRequestsGrid, "Era", "Эпоха");
                AddTextColumn(_myRequestsGrid, "Price", "Цена");
                AddTextColumn(_myRequestsGrid, "Author", "Автор");
                AddTextColumn(_myRequestsGrid, "Country", "Страна");
                AddTextColumn(_myRequestsGrid, "Description", "Описание");
                AddTextColumn(_myRequestsGrid, "Status", "Статус");
                AddTextColumn(_myRequestsGrid, "Phone", "Телефон");
                _myRequestsGrid.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "DeleteRequest", HeaderText = "Действие", Text = "Удалить", UseColumnTextForButtonValue = true
                });
                _myRequestsGrid.CellContentClick += MyRequestsGrid_CellContentClick;
                requestsTab.Controls.Add(_myRequestsGrid);
                tabControl1.TabPages.Add(requestsTab);
                tabControl1.Selected += (s, e) => { if (e.TabPage == requestsTab) LoadMyRequests(); };
            }

            // Гость не видит личные покупки, так как покупка доступна только после входа.
            if (user.Role != Role.Guest)
            {
                var purchasesTab = new TabPage("Покупки") { BackColor = Color.PeachPuff };
                _purchaseSearchBox = new TextBox
                {
                    Name = "PurchaseSearchBox", PlaceholderText = "Поиск по наименованию",
                    Location = new Point(12, 10), Size = new Size(310, 30), Anchor = AnchorStyles.Top | AnchorStyles.Left
                };
                _myPurchasesGrid = CreateProductHistoryGrid(includeBuyer: false);
                _myPurchasesGrid.Location = new Point(8, 48);
                _myPurchasesGrid.Size = new Size(1390, 535);
                _myPurchasesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                purchasesTab.Controls.Add(_myPurchasesGrid);
                purchasesTab.Controls.Add(_purchaseSearchBox);
                _purchaseSearchBox.TextChanged += (s, e) => ShowMyPurchases();
                tabControl1.TabPages.Add(purchasesTab);
                tabControl1.Selected += (s, e) => { if (e.TabPage == purchasesTab) LoadMyPurchases(); };
            }

            if (user.Role == Role.Admin)
            {
                var archiveTab = new TabPage("Архив") { BackColor = Color.PeachPuff };
                _purchaseArchiveGrid = CreateProductHistoryGrid(includeBuyer: true);
                archiveTab.Controls.Add(_purchaseArchiveGrid);
                tabControl1.TabPages.Add(archiveTab);
                tabControl1.Selected += (s, e) => { if (e.TabPage == archiveTab) LoadPurchaseArchive(); };
            }
        }

        private static DataGridView CreateReadOnlyGrid() => new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowTemplate = { Height = 72 }
        };

        private static void AddTextColumn(DataGridView grid, string name, string title)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = title, ReadOnly = true });
        }

        private static DataGridView CreateProductHistoryGrid(bool includeBuyer)
        {
            var grid = CreateReadOnlyGrid();
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.Columns.Add(new DataGridViewImageColumn { Name = "Photo", HeaderText = "Фото", ImageLayout = DataGridViewImageCellLayout.Zoom, FillWeight = 75 });
            AddTextColumn(grid, "Name", "Наименование");
            AddTextColumn(grid, "Category", "Категория");
            AddTextColumn(grid, "Era", "Эпоха");
            AddTextColumn(grid, "Price", "Цена");
            AddTextColumn(grid, "Author", "Автор");
            AddTextColumn(grid, "Country", "Страна");
            AddTextColumn(grid, "Description", "Описание");
            AddTextColumn(grid, "Authenticity", "Подлинность");
            AddTextColumn(grid, "Quantity", "Кол-во");
            if (includeBuyer)
            {
                AddTextColumn(grid, "BuyerName", "Покупатель");
                AddTextColumn(grid, "BuyerLogin", "Логин");
            }
            AddTextColumn(grid, "PurchasedAt", "Дата покупки");
            return grid;
        }

        private void LoadMyRequests()
        {
            if (_myRequestsGrid == null || _customerRequestService == null) return;
            try
            {
                _myRequestsGrid.Rows.Clear();
                foreach (ProposedProduct request in _customerRequestService.GetMyRequests(_userService.CurrentUser.Id))
                {
                    int row = _myRequestsGrid.Rows.Add(request.Name, request.Category, request.Era, request.Price,
                        request.Author, request.Country, request.Description, RequestStatusText(request.Status), request.Phone, "Удалить");
                    _myRequestsGrid.Rows[row].Tag = request;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось загрузить ваши заявки: " + ex.Message, "Мои заявки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string RequestStatusText(string status)
        {
            switch ((status ?? "pending").ToLowerInvariant())
            {
                case "review": return "На рассмотрении";
                case "approved": return "Одобрен";
                case "rejected": return "Отклонён";
                default: return "Отправлено";
            }
        }

        private void MyRequestsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _myRequestsGrid.Columns[e.ColumnIndex].Name != "DeleteRequest") return;
            if (!(_myRequestsGrid.Rows[e.RowIndex].Tag is ProposedProduct request)) return;
            if (MessageBox.Show(this, "Удалить выбранную заявку?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if (_customerRequestService.DeleteMyRequest(request.Id, _userService.CurrentUser.Id))
                LoadMyRequests();
            else
                MessageBox.Show(this, _customerRequestService.LastMessage, "Не удалось удалить заявку", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LoadMyPurchases()
        {
            if (_myPurchasesGrid == null || _purchaseHistoryService == null) return;
            try
            {
                _loadedPurchases = _purchaseHistoryService.GetMyPurchases(_userService.CurrentUser.Id);
                ShowMyPurchases();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось загрузить покупки: " + ex.Message, "Покупки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowMyPurchases()
        {
            if (_myPurchasesGrid == null) return;
            string search = _purchaseSearchBox?.Text.Trim() ?? string.Empty;
            var records = string.IsNullOrEmpty(search) ? _loadedPurchases : _loadedPurchases.Where(record =>
                record.Product.Name.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0);
            PopulatePurchaseGrid(_myPurchasesGrid, records, includeBuyer: false);
        }

        private void LoadPurchaseArchive()
        {
            if (_purchaseArchiveGrid == null || _userService.CurrentUser?.Role != Role.Admin) return;
            try
            {
                PopulatePurchaseGrid(_purchaseArchiveGrid,
                    _purchaseHistoryService.GetArchive(_userService.CurrentUser.Role), includeBuyer: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось загрузить архив покупок: " + ex.Message, "Архив", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void PopulatePurchaseGrid(DataGridView grid, IEnumerable<PurchaseRecord> records, bool includeBuyer)
        {
            ClearGridImages(grid);
            grid.Rows.Clear();
            foreach (PurchaseRecord record in records)
            {
                Image photo = null;
                string photoPath = ResolveProductPhotoPath(record.Product.Foto, record.Product.Name);
                if (!string.IsNullOrEmpty(photoPath))
                {
                    try { photo = Image.FromFile(photoPath); } catch { }
                }

                var values = new List<object>
                {
                    photo, record.Product.Name, record.Product.Categoria, record.Product.Era, record.Product.Prise,
                    record.Product.Autor, record.Product.Country, record.Product.Opisanie,
                    record.Product.AuthenMark, record.Quantity
                };
                if (includeBuyer)
                {
                    values.Add(record.BuyerName);
                    values.Add(record.BuyerLogin);
                }
                values.Add(record.PurchasedAt.ToString("dd.MM.yyyy HH:mm"));
                int row = grid.Rows.Add(values.ToArray());
                grid.Rows[row].Height = 78;
                grid.Rows[row].Tag = record;
            }
        }

        private static void ClearGridImages(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.Value is Image image) image.Dispose();
        }

        private static string? SaveEmbeddedPhotoToCache(string storedPhoto)
        {
            // Вытаскивает фото из БД и создаёт локальный кэш для DataGridView.
            if (string.IsNullOrWhiteSpace(storedPhoto) || !storedPhoto.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                return null;

            int separator = storedPhoto.IndexOf(',');
            if (separator < 0) return null;

            string mimeType = storedPhoto.Substring(5, separator - 5).Split(';')[0].ToLowerInvariant();
            string extension = mimeType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/bmp" => ".bmp",
                _ => string.Empty
            };
            if (string.IsNullOrEmpty(extension)) return null;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(storedPhoto.Substring(separator + 1));
                string hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(imageBytes));
                string directory = Path.Combine(AppContext.BaseDirectory, "images", "proposals");
                Directory.CreateDirectory(directory);
                string imagePath = Path.Combine(directory, "db_" + hash + extension);
                if (!File.Exists(imagePath)) File.WriteAllBytes(imagePath, imageBytes);
                return imagePath;
            }
            catch
            {
                return null;
            }
        }

        private static string ResolveProductPhotoPath(string storedPath, string productName)
        {
            // Сначала использует фото из записи, затем ищет подходящий файл в images.
            if (!string.IsNullOrWhiteSpace(storedPath) &&
                !string.Equals(Path.GetFileName(storedPath), "picture.png", StringComparison.OrdinalIgnoreCase))
            {
                string? embeddedPhoto = SaveEmbeddedPhotoToCache(storedPath);
                if (!string.IsNullOrEmpty(embeddedPhoto)) return embeddedPhoto;

                if (File.Exists(storedPath)) return storedPath;

                string copiedPath = Path.Combine(AppContext.BaseDirectory, storedPath);
                if (File.Exists(copiedPath)) return copiedPath;

                string copiedImage = Path.Combine(AppContext.BaseDirectory, "images", Path.GetFileName(storedPath));
                if (File.Exists(copiedImage)) return copiedImage;
            }

            // Для демонстрационных товаров выбирает отдельное фото по точному названию.
            if (!string.IsNullOrWhiteSpace(productName) &&
                ProductImageFiles.TryGetValue(productName.Trim(), out string? productImage) &&
                !string.IsNullOrWhiteSpace(productImage))
            {
                string productImagePath = Path.Combine(AppContext.BaseDirectory, "images", productImage);
                if (File.Exists(productImagePath)) return productImagePath;
            }

            string normalizedName = (productName ?? string.Empty).ToLowerInvariant();
            string imageName = null;
            if (normalizedName.Contains("ламп")) imageName = "Лампа керосиновая Франция.jpg";
            else if (normalizedName.Contains("буфет") || normalizedName.Contains("комод")) imageName = "Буфет Западная Европпа.jpg";
            else if (normalizedName.Contains("зеркал") || normalizedName.Contains("консол")) imageName = "Консоль Франция.jpg";
            else if (normalizedName.Contains("час") || normalizedName.Contains("будильник")) imageName = "Часы фарфоровые в стиле.jpg";
            else if (normalizedName.Contains("ваз") || normalizedName.Contains("фарфор") || normalizedName.Contains("чайная пара")) imageName = "Вазы.jpg";
            else if (normalizedName.Contains("люстр")) imageName = "Люстра с стиле модерн.jpg";
            else if (normalizedName.Contains("канделябр") || normalizedName.Contains("подсвечник")) imageName = "Канделябры парные из бронзы Франция.jpg";
            else if (normalizedName.Contains("стол") || normalizedName.Contains("стул")) imageName = "Стол Западная Европпа.jpg";

            string imagesDirectory = Path.Combine(AppContext.BaseDirectory, "images");
            if (!string.IsNullOrEmpty(imageName))
            {
                string matchedImage = Path.Combine(imagesDirectory, imageName);
                if (File.Exists(matchedImage)) return matchedImage;
            }

            // Показывает штатную картинку-заполнитель, если точного фото нет.
            string placeholder = Path.Combine(imagesDirectory, "picture.png");
            return File.Exists(placeholder) ? placeholder : null;
        }

        private static string? ResolveProposalPhotoPath(string storedPath)
        {
            // Ищет загруженное клиентом фото по абсолютному или сохранённому относительному пути.
            if (string.IsNullOrWhiteSpace(storedPath)) return null;
            string? embeddedPhoto = SaveEmbeddedPhotoToCache(storedPath);
            if (!string.IsNullOrEmpty(embeddedPhoto)) return embeddedPhoto;
            if (File.Exists(storedPath)) return storedPath;

            string normalizedPath = storedPath.Replace('/', Path.DirectorySeparatorChar);
            string appPath = Path.Combine(AppContext.BaseDirectory, normalizedPath);
            if (File.Exists(appPath)) return appPath;

            string imagePath = Path.Combine(AppContext.BaseDirectory, "images", Path.GetFileName(storedPath));
            return File.Exists(imagePath) ? imagePath : null;
        }

        private void LoadProposals()
        {
            // Загружает очередь из БД и сохраняет модель в Tag каждой строки.
            if (_proposalsGrid == null) return;
            try
            {
                ClearGridImages(_proposalsGrid);
                _proposalsGrid.Rows.Clear();
                foreach (var product in _proposedProductService.GetProposedProducts())
                {
                    Image? photo = null;
                    string? photoPath = ResolveProposalPhotoPath(product.PhotoPath);
                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        try { photo = Image.FromFile(photoPath); } catch { }
                    }
                    int row = _proposalsGrid.Rows.Add(photo, product.Id, product.Name, product.Category, product.Era,
                        product.Price, product.Author, product.Country, product.Description, product.Phone);
                    _proposalsGrid.Rows[row].Height = 90;
                    _proposalsGrid.Rows[row].Tag = product;
                    if (product.Status == "review")
                        _proposalsGrid.Rows[row].DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
            catch (Exception ex) { MessageBox.Show(this, "Не удалось загрузить предложения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ProcessProposal(string action)
        {
            // Проверяет выбор, подтверждает действие и обновляет таблицы после успеха.
            if (_proposalsGrid?.CurrentRow?.Tag is not ProposedProduct product)
            {
                MessageBox.Show(this, "Выберите предложение в таблице.", "Предложения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string prompt = action == "approve" ? "Одобрить товар и добавить его в каталог?" :
                action == "reject" ? "Отклонить и удалить это предложение?" : "Отправить заявку на рассмотрение?";
            if (MessageBox.Show(this, prompt, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (action == "processing")
            {
                bool marked = _proposedProductService.MarkForReview(product.Id);
                MessageBox.Show(this, _proposedProductService.LastMessage, marked ? "Готово" : "Не выполнено",
                    MessageBoxButtons.OK, marked ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                if (marked) LoadProposals();
                return;
            }
            bool success = action == "approve" ? _proposedProductService.ApproveProduct(product.Id) : _proposedProductService.RejectProduct(product.Id);
            MessageBox.Show(this, _proposedProductService.LastMessage, success ? "Готово" : "Не выполнено",
                MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (success)
            {
                LoadProposals();
                if (action == "approve") LoadProducts();
            }
        }

        private void ButtonProposeProduct_Click(object sender, EventArgs e)
        {
            // Гостю сначала нужно войти или зарегистрироваться, чтобы оставить заявку.
            if (_userService.CurrentUser?.Role == Role.Guest)
            {
                MessageBox.Show(this,
                    "Чтобы предложить товар, войдите в аккаунт или зарегистрируйтесь. Сейчас откроется окно авторизации.",
                    "Требуется авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _userService.Logout();
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Authorization authorizationForm)
                    {
                        authorizationForm.Show();
                        authorizationForm.BringToFront();
                        Close();
                        return;
                    }
                }
                MessageBox.Show(this, "Окно авторизации не найдено. Перезапустите приложение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Открывает форму отправки новой заявки для авторизованного пользователя.
            using (var form = new ProposeProductForm(_proposedProductRepository, _userService.CurrentUser?.Id))
                form.ShowDialog(this);
            LoadMyRequests();
        }

        private void LoadUserInfo()
        {
            // Показывает логин, имя и роль текущей сессии.
            if (_userService.CurrentUser != null)
            {
                label1.Text = $"Пользователь: {_userService.CurrentUser.Login}\nФИО: {_userService.CurrentUser.FullName}\nРоль: {User.RoleToDisplayString(_userService.CurrentUser.Role)}";
            }
            else
            {
                label1.Text = "Вы не авторизованы.";
            }
        }

        // Настройка колонок для DataGridView на вкладке Корзины
        private void SetupCartGrid()
        {
            // Создаёт колонки для списка товаров в корзине.
            if (DataGridView2 == null) return;

            DataGridView2.Columns.Clear();
            DataGridView2.AutoGenerateColumns = false;

            DataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Товар" });
            DataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Цена" });
            DataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qty", HeaderText = "Кол-во" });
            DataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Сумма" });
            if (_userService.CurrentUser != null && _userService.CurrentUser.Id > 0)
            {
                foreach (var item in _dbConnection.GetCartItems(_userService.CurrentUser.Id))
                    _cart.AddProduct(item.Product, item.Quantity);
                UpdateCartView();
            }
            var buyButton = new Button
            {
                Name = "btnBuy", Text = "Купить", Location = new Point(720, 95), Size = new Size(165, 56),
                BackColor = Color.Sienna, ForeColor = Color.PeachPuff, FlatStyle = FlatStyle.Flat
            };
            buyButton.Click += BuyButton_Click;
            tabPage3.Controls.Add(buyButton);

        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            // Для оплаты сначала требует войти в аккаунт, как и при отправке заявки.
            if (_userService.CurrentUser?.Role == Role.Guest)
            {
                MessageBox.Show(this,
                    "Чтобы купить товар, войдите в аккаунт или зарегистрируйтесь. Сейчас откроется окно авторизации.",
                    "Требуется авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _userService.Logout();
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Authorization authorizationForm)
                    {
                        authorizationForm.Show();
                        authorizationForm.BringToFront();
                        Close();
                        return;
                    }
                }
                MessageBox.Show(this, "Окно авторизации не найдено. Перезапустите приложение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Запрашивает карту, телефон, адрес и подтверждение, затем удаляет купленные товары из каталога.
            if (_cart.Items.Count == 0)
            {
                MessageBox.Show(this, "Корзина пуста.", "Покупка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var paymentForm = new PaymentForm())
            {
                if (paymentForm.ShowDialog(this) != DialogResult.OK) return;
            }

            try
            {
                if (!_purchaseHistoryService.CompletePurchase(_userService.CurrentUser, _cart.Items))
                {
                    MessageBox.Show(this, _purchaseHistoryService.LastMessage + " Некоторые товары могли стать недоступны.", "Покупка не завершена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadProducts();
                    return;
                }

                _cart.Clear();
                UpdateCartView();
                LoadProducts();
                LoadMyPurchases();
                LoadPurchaseArchive();
                MessageBox.Show(this, "Поздравляем с покупкой! Спасибо, что выбрали наш магазин.", "Покупка оформлена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось оформить покупку: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            // Загружает каталог из БД и показывает данные в таблице товаров.
            try
            {
                _allProducts = _dbConnection.GetAllTovars();
                PopulateFilterChoices();
                ApplyProductFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Логика кнопки "Добавить в корзину" ---
        private void KorzinaButton_Click(object sender, EventArgs e)
        {
            // Добавляет выбранную строку каталога в локальную корзину.
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар из списка.");
                return;
            }

            if (dataGridView1.CurrentRow.Tag is Tovar selectedProduct)
            {
                if (_userService.CurrentUser != null && _userService.CurrentUser.Id > 0 &&
                    !_dbConnection.AddCartItem(_userService.CurrentUser.Id, selectedProduct.Id))
                {
                    MessageBox.Show(this, "Не удалось сохранить товар в корзине.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _cart.AddProduct(selectedProduct);
                UpdateCartView();

                if (tabControl1.TabPages.ContainsKey("tabPageCart"))
                {
                    tabControl1.SelectedTab = tabPage3;
                }
            }
        }

        // Метод обновления отображения корзины
        private void UpdateCartView()
        {
            // Перерисовывает содержимое корзины и сумму заказа.
            if (DataGridView2 == null) return;

            DataGridView2.Rows.Clear();

            foreach (var item in _cart.Items)
            {
                int rowIndex = DataGridView2.Rows.Add(
                    item.Product.Name,
                    item.Product.Prise,
                    item.Quantity,
                    item.TotalPrice
                );

                // !!! ИСПРАВЛЕНО: Сохраняем объект CartItem в Tag, чтобы работало удаление по кнопке !!!
                DataGridView2.Rows[rowIndex].Tag = item;
            }

            // Если есть Label для итоговой суммы
            lblTotalSum.Text = $"Итого: {_cart.GetTotalSum()} руб.";
        }

        // Обработка кликов по таблице товаров
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Обрабатывает действия внутри таблицы каталога.
            if (e.RowIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "SaveBtn")
            {
                MessageBox.Show("Функция сохранения в разработке.", "Инфо");
            }
        }

        // Обработка кликов по таблице корзины (для кнопки "X" внутри таблицы)
        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Удаляет товар, если нажата кнопка удаления в строке корзины.
            if (e.RowIndex < 0) return;

            // Если нажали на колонку с кнопкой удаления
            if (DataGridView2.Columns[e.ColumnIndex].Name == "DeleteBtn")
            {
                // Получаем объект из Tag строки, по которой кликнули
                if (DataGridView2.Rows[e.RowIndex].Tag is CartItem itemToRemove)
                {
                    if (!RemoveCartItemFromDatabase(itemToRemove)) return;
                    _cart.RemoveProduct(itemToRemove.Product.Name);
                    UpdateCartView();
                }
            }
        }

        // Логика отдельной кнопки удаления (если она есть на форме)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Удаляет из корзины выбранную строку.
            if (DataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.");
                return;
            }

            // Теперь это сработает, так как мы добавили строку в UpdateCartView: Tag = item
            if (DataGridView2.CurrentRow.Tag is CartItem itemToRemove)
            {
                if (!RemoveCartItemFromDatabase(itemToRemove)) return;
                _cart.RemoveProduct(itemToRemove.Product.Name);
                UpdateCartView();
            }
            else
            {
                MessageBox.Show("Ошибка данных.");
            }
        }

        private bool RemoveCartItemFromDatabase(CartItem item)
        {
            // Удаление сначала подтверждается в БД, чтобы список не расходился после ошибки.
            int userId = _userService.CurrentUser?.Id ?? 0;
            if (userId <= 0) return true;
            try
            {
                if (_dbConnection.RemoveCartItem(userId, item.Product.Id)) return true;
                MessageBox.Show(this, "Не удалось удалить позицию из сохранённой корзины.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось изменить корзину: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Завершает текущую сессию и возвращает окно входа.
            _userService.Logout();
            foreach (Form form in Application.OpenForms)
            {
                if (form is Authorization authForm)
                {
                    authForm.Show();
                    break;
                }
            }
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Освобождает изображения, загруженные в таблицу каталога.
            ClearGridImages(dataGridView1);
            if (_myPurchasesGrid != null) ClearGridImages(_myPurchasesGrid);
            if (_purchaseArchiveGrid != null) ClearGridImages(_purchaseArchiveGrid);
        }


    }
}
