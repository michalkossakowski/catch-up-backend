-- USERS
-- BASIC TYPE USERS
INSERT INTO [Users] ([Id], [Name], [Surname], [Email], [Password], [Type], [Position], [State], [Counters], [AvatarId]) 
VALUES 
(N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Admin', N'Adminski', N'admin@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Admin', N'Admin', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
(N'04D68924-791D-4022-F2E3-08DD33FC8FD5', N'Mentor', N'Mentorski', N'mentor@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Mentor', 0, N'{"AssignNewbiesCount":1,"CheckedTasksCount":2,"CreatedTasksCount":3,"CreatedSchoolingsCount":4}', NULL),
(N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'Newbie', N'Newbieski', N'newbie@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Newbie', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
(N'8d2d867e-a31c-4bef-827a-75c9c1703a23', N'HRek', N'HRowski', N'hr@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'HR', N'HR', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL);

-- OTHER USERS
INSERT INTO [Users] 
    ([Id], [Name], [Surname], [Email], [Password], [Type], [Position], [State], [Counters], [AvatarId])
VALUES 
    (N'30943099-da50-4271-931c-08dd3251ce0a', N'Adam', N'Małysz', N'adam@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Frontend', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', N'Zbigniew', N'Stonóg', N'zbigniew@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Menager', N'Backend', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', N'Krzysztof', N'Krawczyk', N'krawczyk@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Frontend', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'12a34bcd-56ef-7890-1234-56789abcdef0', N'Anna', N'Nowak', N'anna.nowak@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Design', 0, N'{"AssignNewbiesCount":2,"CheckedTasksCount":15,"CreatedTasksCount":5,"CreatedSchoolingsCount":1}', NULL),
    (N'22334455-6677-8899-aabb-ccddeeff0011', N'Marek', N'Kowalski', N'marek.kowalski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Admin', N'DevOps', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":10,"CreatedSchoolingsCount":2}', NULL),
    (N'abcdef01-2345-6789-0123-456789abcdef', N'Piotr', N'Szymański', N'piotr.szymanski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Testing', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":1,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'34567890-1234-5678-90ab-cdef01234567', N'Joanna', N'Wiśniewska', N'joanna.wisniewska@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Marketing', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'45678901-2345-6789-0abc-def123456789', N'Karolina', N'Wróbel', N'karolina.wrobel@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'HR', 0, N'{"AssignNewbiesCount":5,"CheckedTasksCount":20,"CreatedTasksCount":3,"CreatedSchoolingsCount":1}', NULL),
    (N'56789012-3456-7890-abcd-ef0123456789', N'Łukasz', N'Dąbrowski', N'lukasz.dabrowski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Admin', N'Data Science', 0, N'{"AssignNewbiesCount":1,"CheckedTasksCount":25,"CreatedTasksCount":7,"CreatedSchoolingsCount":3}', NULL),
    (N'67890123-4567-8901-bcde-f0123456789a', N'Monika', N'Kaczmarek', N'monika.kaczmarek@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Sales', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL),
    (N'78901234-5678-9012-cdef-0123456789ab', N'Artur', N'Kwiatkowski', N'artur.kwiatkowski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Backend', 0, N'{"AssignNewbiesCount":4,"CheckedTasksCount":30,"CreatedTasksCount":10,"CreatedSchoolingsCount":5}', NULL),
    (N'89012345-6789-0123-def0-123456789abc', N'Tomasz', N'Jankowski', N'tomasz.jankowski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Frontend', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":1,"CreatedSchoolingsCount":0}', NULL),
    (N'90123456-7890-1234-ef01-23456789abcd', N'Magdalena', N'Lewandowska', N'magdalena.lewandowska@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'UI/UX', 0, N'{"AssignNewbiesCount":3,"CheckedTasksCount":10,"CreatedTasksCount":4,"CreatedSchoolingsCount":2}', NULL),
    (N'01234567-8901-2345-f012-3456789abcde', N'Paweł', N'Kamiński', N'pawel.kaminski@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Manager', N'Product Management', 0, N'{"AssignNewbiesCount":2,"CheckedTasksCount":5,"CreatedTasksCount":6,"CreatedSchoolingsCount":3}', NULL),
    (N'12345678-9012-3456-0123-456789abcdef', N'Aleksandra', N'Zielińska', N'aleksandra.zielinska@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Admin', N'IT Support', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":15,"CreatedTasksCount":3,"CreatedSchoolingsCount":1}', NULL),
    (N'23456789-0123-4567-1234-56789abcdef0', N'Rafał', N'Nowicki', N'rafal.nowicki@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Cybersecurity', 0, N'{"AssignNewbiesCount":6,"CheckedTasksCount":22,"CreatedTasksCount":9,"CreatedSchoolingsCount":4}', NULL),
    (N'34567890-1234-5678-2345-6789abcdef01', N'Natalia', N'Wójcik', N'natalia.wojcik@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'Content Writing', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":2,"CreatedSchoolingsCount":0}', NULL),
    (N'45678901-2345-6789-3456-789abcdef012', N'Bartłomiej', N'Kulesza', N'bartlomiej.kulesza@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'HR', N'Cloud Engineering', 0, N'{"AssignNewbiesCount":1,"CheckedTasksCount":18,"CreatedTasksCount":5,"CreatedSchoolingsCount":3}', NULL),
    (N'56789012-3456-7890-4567-89abcdef0123', N'Małgorzata', N'Ostrowska', N'malgorzata.ostrowska@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Mentor', N'Machine Learning', 0, N'{"AssignNewbiesCount":2,"CheckedTasksCount":25,"CreatedTasksCount":8,"CreatedSchoolingsCount":2}', NULL),
    (N'67890123-4567-8901-5678-9abcdef01234', N'Andrzej', N'Lis', N'andrzej.lis@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'HR', N'Operations', 0, N'{"AssignNewbiesCount":1,"CheckedTasksCount":12,"CreatedTasksCount":7,"CreatedSchoolingsCount":4}', NULL),
    (N'78901234-5678-9012-6789-abcdef012345', N'Joanna', N'Sikora', N'joanna.sikora@catchup.com', N'vdIpf5NVDwFFLL2DjCdvDdIvSYtGYTlPFSiriNbmPm8=', N'Newbie', N'SEO', 0, N'{"AssignNewbiesCount":0,"CheckedTasksCount":0,"CreatedTasksCount":0,"CreatedSchoolingsCount":0}', NULL);

-- CATEGORIES
SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] 
    ([Id], [Name], [State]) 
VALUES 
    (1, N'Programowanie', 0),
    (2, N'Projektowanie', 0),
    (3, N'Marketing', 0),
    (4, N'Analiza Danych', 0),
    (5, N'Testowanie Oprogramowania', 0),
    (6, N'Kursy Online', 0),
    (7, N'Szkolenia Zawodowe', 0),
    (8, N'Programowanie i Rozwój Oprogramowania', 0),
    (9, N'Chmura i Technologie Sieciowe', 0),
    (10, N'Zarządzanie i Przedsiębiorczość', 0),
    (11, N'Zarządzanie Zasobami Ludzkimi', 0),
    (12, N'Internet Rzeczy (IoT)', 0);
SET IDENTITY_INSERT [Categories] OFF;

-- FAQS
SET IDENTITY_INSERT [Faqs] ON
INSERT INTO [Faqs] 
    ([Id], [Question], [Answer], [CreatorId], [MaterialId], [State])
VALUES 
    (1, N'Jakie są zalety korzystania z naszej platformy?', N'Nasza platforma oferuje wiele zalet, które wyróżniają ją na tle konkurencji. Przede wszystkim, zapewnia dostęp do szerokiego wachlarza narzędzi, które umożliwiają użytkownikom szybkie i efektywne zarządzanie swoimi zadaniami. Ponadto, dzięki intuicyjnemu interfejsowi, korzystanie z platformy jest bardzo proste, nawet dla osób, które nie mają doświadczenia w technologii. Platforma jest również regularnie aktualizowana, co zapewnia jej nowoczesność i zgodność z najnowszymi trendami rynkowymi. Użytkownicy mogą liczyć na wsparcie techniczne oraz szybki dostęp do pomocy w razie problemów.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (2, N'Czym różni się nasza oferta od konkurencji?', N'Nasza oferta różni się od konkurencji przede wszystkim podejściem do klienta. Zamiast skupiać się tylko na sprzedaży, my staramy się zrozumieć potrzeby naszych użytkowników i dostarczać im rozwiązania dopasowane do ich indywidualnych wymagań. Nasza platforma umożliwia dostosowanie do konkretnych branż, co zapewnia jeszcze lepszą funkcjonalność. Dodatkowo, oferujemy elastyczne opcje płatności oraz wsparcie na każdym etapie współpracy, co sprawia, że nasza oferta jest bardziej przystępna i kompleksowa. Nasza dedykowana obsługa klienta jest dostępna 24/7, co daje poczucie bezpieczeństwa i pewności, że użytkownicy zawsze otrzymają pomoc, kiedy jej potrzebują.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (3, N'Jakie są główne funkcje naszej aplikacji mobilnej?', N'Aplikacja mobilna naszej platformy oferuje szereg funkcji, które umożliwiają wygodne zarządzanie zadaniami i projektami w dowolnym miejscu i czasie. Użytkownicy mogą synchronizować swoje dane z kontem, otrzymywać powiadomienia o nadchodzących wydarzeniach, a także mieć dostęp do szczegółowych raportów i statystyk. Aplikacja pozwala na łatwe przypisywanie zadań, komunikowanie się z zespołem oraz śledzenie postępów pracy. Dodatkowo, oferuje funkcje offline, dzięki której użytkownicy mogą kontynuować pracę nawet w miejscach bez dostępu do internetu. Aplikacja jest dostępna na urządzenia z systemami Android oraz iOS i jest regularnie aktualizowana, aby zapewnić najwyższą jakość działania.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (4, N'Jakie plany rozwoju ma nasza firma na przyszłość?', N'Nasza firma stawia na innowacje i ciągły rozwój, dlatego mamy ambitne plany na przyszłość. W najbliższych latach planujemy rozszerzenie naszej oferty o nowe funkcjonalności, które będą jeszcze bardziej dostosowane do potrzeb naszych użytkowników. Pracujemy nad rozwojem sztucznej inteligencji i automatyzacji, aby jeszcze bardziej usprawnić nasze rozwiązania. Planujemy również ekspansję na nowe rynki międzynarodowe, co pozwoli nam dotrzeć do szerszej grupy klientów. Naszym celem jest stale podnoszenie jakości naszych usług, aby oferować użytkownikom narzędzia, które będą ich wspierały w codziennej pracy i pozwalały osiągać jeszcze lepsze wyniki.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (5, N'Jakie technologie są wykorzystywane w naszej platformie?', N'Nasza platforma opiera się na nowoczesnych technologiach, które zapewniają jej niezawodność, skalowalność i bezpieczeństwo. Korzystamy z najnowszych rozwiązań chmurowych, co pozwala na elastyczne zarządzanie danymi oraz ich łatwą synchronizację w czasie rzeczywistym. Dodatkowo, platforma jest zbudowana na bazie technologii takich jak JavaScript, React, Node.js oraz Python, co zapewnia jej wysoką wydajność i możliwość łatwego rozszerzania funkcji. Dbamy także o bezpieczeństwo danych, stosując szyfrowanie na każdym etapie ich przetwarzania. Nasze usługi są zgodne z najlepszymi standardami branżowymi, w tym z regulacjami RODO, co daje naszym użytkownikom pewność, że ich dane są w pełni chronione.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (6, N'Jakie są korzyści z integracji naszej platformy z innymi narzędziami?', N'Integracja naszej platformy z innymi popularnymi narzędziami umożliwia użytkownikom jeszcze łatwiejsze zarządzanie swoimi zadaniami i projektami. Dzięki możliwości synchronizacji z aplikacjami, które już są wykorzystywane przez firmy, użytkownicy mogą zaoszczędzić czas i uniknąć konieczności ręcznego wprowadzania danych. Dodatkowo, integracja pozwala na tworzenie bardziej złożonych procesów, automatyzując wiele czynności, co przekłada się na wzrost efektywności. Nasza platforma obsługuje integracje z szeroką gamą narzędzi, takich jak systemy CRM, aplikacje do zarządzania projektami, czy też narzędzia analityczne, co sprawia, że jest ona wyjątkowo elastyczna i dostosowana do różnych branż.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (7, N'Co to jest model subskrypcyjny i jak działa w naszej platformie?', N'Model subskrypcyjny naszej platformy daje użytkownikom możliwość dostępu do pełnej funkcjonalności narzędzi w ramach różnych planów subskrypcyjnych. W zależności od wybranego planu, użytkownicy mają dostęp do różnych poziomów funkcji, takich jak większa ilość przechowywanych danych, więcej dostępnych integracji, czy też dedykowane wsparcie techniczne. Model subskrypcyjny daje także elastyczność w doborze odpowiedniego pakietu, co umożliwia firmom dostosowanie kosztów do ich rzeczywistych potrzeb. Subskrypcje są odnawiane co miesiąc lub co rok, w zależności od preferencji użytkownika, a dzięki naszemu systemowi można łatwo zarządzać płatnościami oraz planować rozwój w długim okresie.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (8, N'Jakie opcje dostosowywania są dostępne w naszej platformie?', N'Nasza platforma oferuje szeroką gamę opcji dostosowywania, dzięki którym użytkownicy mogą stworzyć środowisko pracy, które najlepiej odpowiada ich potrzebom. Możliwości dostosowywania obejmują m.in. personalizację interfejsu użytkownika, możliwość tworzenia własnych kategorii i tagów, a także ustawienia powiadomień, które mogą być dostosowane do konkretnych zadań i projektów. Dodatkowo, platforma umożliwia tworzenie niestandardowych raportów i statystyk, które pozwalają na lepsze monitorowanie wyników. Dzięki temu, niezależnie od branży czy wielkości firmy, każdy użytkownik ma możliwość stworzenia optymalnego środowiska pracy, które zwiększa produktywność.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (9, N'Jakie są główne wyzwania związane z cyfryzacją procesów biznesowych?', N'Cyfryzacja procesów biznesowych to proces, który wiąże się z wieloma wyzwaniami, ale także z ogromnym potencjałem do poprawy efektywności i konkurencyjności firmy. Jednym z głównych wyzwań jest integracja nowych technologii z istniejącymi systemami. Wiele firm korzysta z rozwiązań legacy, które nie zawsze łatwo współpracują z nowoczesnymi narzędziami. Kolejnym wyzwaniem jest zarządzanie danymi – konieczność zapewnienia ich bezpieczeństwa oraz odpowiedniego przechowywania staje się coraz bardziej skomplikowana w miarę wzrostu ilości danych. Współczesne firmy muszą także zadbać o odpowiednie szkolenia dla swoich pracowników, aby mogli efektywnie wykorzystywać nowe technologie. Pomimo tych trudności, cyfryzacja może przynieść ogromne korzyści, takie jak zwiększenie efektywności procesów, poprawa komunikacji wewnętrznej, a także lepsze podejmowanie decyzji dzięki dostępowi do danych w czasie rzeczywistym.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (10, N'Jakie są różnice między wersją darmową a płatną naszej platformy?', N'Wersja darmowa naszej platformy oferuje podstawowe funkcje, które pozwalają na rozpoczęcie pracy i testowanie jej możliwości. Użytkownicy mogą korzystać z narzędzi do zarządzania zadaniami, tworzyć projekty, przypisywać zadania do członków zespołu oraz monitorować postępy. Jednak w wersji darmowej dostępnych jest tylko ograniczona ilość miejsca na dane i brak dostępu do zaawansowanych funkcji analitycznych i raportowych. Wersja płatna, z kolei, daje dostęp do pełnej funkcjonalności platformy, w tym rozszerzonej przestrzeni dyskowej, integracji z innymi narzędziami, zaawansowanych raportów, a także priorytetowego wsparcia technicznego. Dodatkowo, w wersji płatnej dostępne są opcje automatyzacji procesów oraz bardziej rozbudowane opcje dostosowywania platformy do specyficznych potrzeb firmy.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (11, N'Co zrobić, jeśli napotkam problemy techniczne podczas korzystania z platformy?', N'Jeśli napotkasz jakiekolwiek problemy techniczne podczas korzystania z naszej platformy, pierwszym krokiem powinno być sprawdzenie sekcji FAQ oraz materiałów pomocy dostępnych na naszej stronie internetowej. Wiele kwestii technicznych, takich jak problemy z logowaniem, synchronizacja danych, czy też błędami w działaniu funkcji, może zostać szybko rozwiązanych dzięki tym zasobom. Jeśli problem nie zostanie rozwiązany, możesz skontaktować się z naszym zespołem wsparcia technicznego, który jest dostępny 24/7. Wystarczy wypełnić formularz kontaktowy na stronie lub skontaktować się z nami telefonicznie, a nasz zespół podejmie natychmiastowe działania w celu rozwiązania problemu. Nasza obsługa klienta dokłada wszelkich starań, aby zapewnić jak najszybsze i najskuteczniejsze rozwiązanie problemu, dbając o minimalizację przestojów i zapewniając komfort użytkowania naszej platformy.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (12, N'Czy platforma jest bezpieczna w użyciu i jak chronimy dane użytkowników?', N'Tak, nasza platforma jest wyjątkowo bezpieczna w użyciu, ponieważ stosujemy najnowsze technologie zabezpieczeń, aby chronić dane użytkowników. Wszystkie dane są szyfrowane zarówno w trakcie przesyłania, jak i przechowywania na naszych serwerach, co minimalizuje ryzyko ich przechwycenia przez osoby nieuprawnione. Ponadto, platforma jest zgodna z międzynarodowymi standardami ochrony danych, w tym z regulacjami RODO, co oznacza, że zapewniamy pełną kontrolę nad danymi osobowymi naszych użytkowników. Regularnie przeprowadzamy audyty bezpieczeństwa, aby upewnić się, że nasza platforma jest wolna od luk bezpieczeństwa, a także oferujemy użytkownikom opcje zarządzania dostępem, w tym wielopoziomowe uwierzytelnianie i możliwość nadawania różnych uprawnień w zależności od roli w organizacji. Dzięki tym środkom nasi użytkownicy mogą mieć pewność, że ich dane są odpowiednio chronione i zabezpieczone przed nieautoryzowanym dostępem.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (13, N'Jakie są kluczowe zalety automatyzacji procesów w naszej platformie?', N'Automatyzacja procesów jest jedną z kluczowych funkcji naszej platformy, która pozwala użytkownikom zaoszczędzić czas, zminimalizować błędy ludzkie oraz zwiększyć efektywność operacyjną. Dzięki zaawansowanym narzędziom do automatyzacji, użytkownicy mogą zaplanować i zrealizować zadania bez konieczności ręcznego ich monitorowania. Na przykład, nasza platforma umożliwia automatyczne przypisywanie zadań do odpowiednich członków zespołu na podstawie ich dostępności oraz priorytetu zadania. Dodatkowo, procesy raportowania i analizowania wyników są całkowicie zautomatyzowane, co pozwala na szybki dostęp do danych w czasie rzeczywistym. Automatyzacja procesów pomaga również w eliminowaniu zbędnych czynności manualnych, dzięki czemu użytkownicy mogą skupić się na bardziej strategicznych działaniach. Zaletą automatyzacji jest także optymalizacja wykorzystania zasobów – procesy są wykonywane szybciej, co przekłada się na lepszą alokację czasu i pracy zespołów.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (14, N'Czy nasza platforma jest skalowalna i jak dostosowuje się do rosnących potrzeb firm?', N'Nasza platforma została zaprojektowana z myślą o skalowalności, aby mogła rosnąć razem z Twoją firmą. Dzięki elastycznej architekturze i możliwości rozbudowy, platforma może z łatwością dostosować się do zwiększającej się liczby użytkowników, danych czy zadań. Skalowalność naszej platformy obejmuje zarówno aspekty techniczne, jak i funkcjonalne. Technicznie, platforma jest zbudowana na chmurze, co oznacza, że zasoby obliczeniowe oraz przestrzeń dyskowa można łatwo dostosować do aktualnych potrzeb użytkowników, bez konieczności inwestowania w kosztowną infrastrukturę. Ponadto, funkcjonalnie platforma oferuje różne poziomy dostępu, dzięki czemu zarówno małe firmy, jak i duże przedsiębiorstwa mogą dostosować jej ustawienia do swoich specyficznych potrzeb. W przypadku firm, które rozwijają się i potrzebują nowych funkcji, nasza platforma pozwala na łatwą integrację z innymi narzędziami i systemami, co zapewnia płynne rozszerzanie jej funkcjonalności.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (15, N'Jakie korzyści płyną z pełnej integracji z systemami ERP i CRM?', N'Pełna integracja naszej platformy z systemami ERP (Enterprise Resource Planning) oraz CRM (Customer Relationship Management) przynosi firmom szereg korzyści, w tym znaczne usprawnienie procesów biznesowych, poprawę efektywności zarządzania danymi oraz lepsze podejmowanie decyzji. Integracja z systemem ERP umożliwia automatyczne synchronizowanie danych dotyczących zapasów, finansów i zarządzania zasobami w jednym, spójnym systemie. Dzięki temu, informacje są dostępne w czasie rzeczywistym, co pozwala na szybsze i dokładniejsze podejmowanie decyzji w obszarze zarządzania łańcuchem dostaw, produkcją czy finansami. Integracja z systemem CRM z kolei pozwala na zarządzanie relacjami z klientami i automatyzację procesów sprzedaży, marketingu oraz obsługi klienta. Synchronizacja danych między systemami CRM i ERP zapewnia, że wszystkie działy firmy mają dostęp do tych samych, najnowszych informacji, co prowadzi do lepszej komunikacji i współpracy wewnętrznej. Korzyści te przekładają się na zwiększoną produktywność, mniejsze ryzyko błędów oraz wyższy poziom satysfakcji klientów, dzięki szybkiemu i sprawnemu reagowaniu na ich potrzeby.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (16, N'Jakie mechanizmy ochrony prywatności danych użytkowników zostały wdrożone na naszej platformie?', N'Bezpieczeństwo i ochrona prywatności danych użytkowników są dla nas priorytetem, dlatego nasza platforma została wyposażona w szereg mechanizmów ochrony danych, które zapewniają, że wszystkie informacje przechowywane na systemie są bezpieczne i zgodne z obowiązującymi przepisami prawa. Po pierwsze, stosujemy najnowsze standardy szyfrowania zarówno dla danych przesyłanych, jak i przechowywanych w naszej bazie danych. Każda interakcja użytkownika z platformą jest zabezpieczona protokołem HTTPS, co zapobiega przechwytywaniu danych w trakcie transmisji. Dodatkowo, dane przechowywane na naszych serwerach są szyfrowane przy użyciu zaawansowanych algorytmów, co zapewnia, że dostęp do nich mają tylko uprawnione osoby. Ponadto, platforma jest zgodna z regulacjami RODO (Rozporządzenie o Ochronie Danych Osobowych), co oznacza, że użytkownicy mają pełną kontrolę nad swoimi danymi i mogą w każdej chwili je edytować lub usunąć. Wprowadziliśmy także mechanizmy wielopoziomowego uwierzytelniania (MFA), które zapewniają, że dostęp do wrażliwych informacji mają tylko osoby posiadające odpowiednie uprawnienia. Każdy użytkownik ma możliwość ustalenia poziomu dostępu dla swoich współpracowników, co pozwala na jeszcze lepszą kontrolę nad dostępem do danych w organizacji. Dodatkowo, regularnie przeprowadzamy audyty bezpieczeństwa oraz testy penetracyjne, aby upewnić się, że nasza platforma jest wolna od jakichkolwiek luk w zabezpieczeniach.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0),
    (17, N'Jakie szkolenia są dostępne dla użytkowników platformy i jak mogę z nich skorzystać?', N'Aby zapewnić naszym użytkownikom jak najlepsze doświadczenie w korzystaniu z platformy, oferujemy szeroki wachlarz szkoleń i materiałów edukacyjnych, które pomagają w pełni wykorzystać jej funkcjonalności. Nasze szkolenia obejmują zarówno podstawowe kursy wprowadzające, jak i zaawansowane sesje dotyczące specyficznych funkcji i narzędzi dostępnych na platformie. Szkolenia te są dostępne w różnych formatach: od kursów online, które można odbyć w dowolnym czasie, przez webinaria na żywo prowadzone przez ekspertów, po indywidualne sesje konsultacyjne dla większych firm. W ramach tych szkoleń omawiamy m.in. jak efektywnie zarządzać projektami i zadaniami, jak korzystać z zaawansowanych narzędzi do analizy danych, oraz jak integrować naszą platformę z innymi systemami. Wszystkie materiały szkoleniowe są dostosowane do poziomu zaawansowania użytkownika, dzięki czemu zarówno nowi użytkownicy, jak i ci bardziej doświadczeni, mogą znaleźć coś odpowiedniego dla siebie. Dodatkowo, dla firm oferujemy możliwość organizowania szkoleń zamkniętych, które mogą być dostosowane do specyficznych potrzeb i procesów zachodzących w danej organizacji. Użytkownicy mogą również skorzystać z bazy wiedzy dostępnej na naszej stronie internetowej, która zawiera artykuły, filmy instruktażowe oraz często zadawane pytania (FAQ), pomagające rozwiązać najczęstsze problemy.', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', NULL, 0);
SET IDENTITY_INSERT [Faqs] OFF

-- MATERIALS
SET IDENTITY_INSERT [Materials] ON;
INSERT INTO [Materials] 
    ([Id], [Name], [State]) 
VALUES 
    (1, N'Wstęp do Programowania w Pythonie', 0),
    (2, N'Zaawansowane Techniki Projektowania UX', 0),
    (3, N'Podstawy Marketingu Internetowego', 0),
    (4, N'Przewodnik po Algorytmach w Informatyce', 0),
    (5, N'Wprowadzenie do Sztucznej Inteligencji', 0),
    (6, N'Efektywne Zarządzanie Czasem w Pracy', 0),
    (7, N'Nauka Języka Niemieckiego - Lekcja 1', 0),
    (8, N'Praktyczny Kurs Grafiki Komputerowej', 0),
    (9, N'Analiza Danych z użyciem Excela', 0),
    (10, N'Budowanie Aplikacji Webowych z Reactem', 0),
    (11, N'Podstawy HTML5 i CSS3', 0),
    (12, N'Tworzenie Aplikacji Mobilnych w React Native', 0),
    (13, N'Kurs SQL: Podstawy i Zaawansowane Techniki', 0),
    (14, N'Bezpieczeństwo w Sieci: Ochrona przed Cyberzagrożeniami', 0),
    (15, N'Podstawy Programowania w Javie', 0),
    (16, N'Zarządzanie Chmurą: AWS vs Azure', 0),
    (17, N'Wprowadzenie do Testowania Oprogramowania', 0);
SET IDENTITY_INSERT [Materials] OFF;

-- FILES
SET IDENTITY_INSERT [Files] ON
INSERT INTO [Files] 
    ([Id], [Name], [Type], [Source], [State]) 
VALUES
    (1, N'1529301479.2217.jpg', N'image/jpeg', N'.\.\Files\96e38e64-cc6c-4950-b322-42b7604513ac_1529301479.2217.jpg', 0),
    (2, N'Ryc.-1-1.png', N'image/png', N'.\.\Files\65999e19-8591-480d-87a3-b287919edb25_Ryc.-1-1.png', 0),
    (3, N'Engineering_large.jpg', N'image/jpeg', N'.\.\Files\014dad7c-db21-4884-b65a-b02619d5f3be_Engineering_large.jpg', 0),
    (4, N'Wprowadzenie do języka Python.pdf', N'application/pdf', N'.\.\Files\77a41432-98f3-40c9-b5f7-5addd6a13640_Wprowadzenie do języka Python.pdf', 0),
    (5, N'Wprowadzenie do języka Python.pdf', N'application/pdf', N'.\.\Files\eb13b886-13ca-43a6-bf7a-5be2e189b369_Wprowadzenie do języka Python.pdf', 0),
    (6, N'E-book-Dobre-praktyki-UX-UI.pdf', N'application/pdf', N'.\.\Files\182f94da-984f-40d4-b84e-f08b7c68dd29_E-book-Dobre-praktyki-UX-UI.pdf', 0),
    (7, N'e-commerce_poradnik_marketing_online.pdf', N'application/pdf', N'.\.\Files\16843565-7a48-4d9b-998d-b8dcf6d85d4b_e-commerce_poradnik_marketing_online.pdf', 0),
    (8, N'Algorytmy-i-struktury-danych.-Cwiczenia.pdf', N'application/pdf', N'.\.\Files\d99ccba1-be82-4abe-8307-ea1b4823eca7_Algorytmy-i-struktury-danych.-Cwiczenia.pdf', 0),
    (9, N'podręcznik_algorytmy.pdf', N'application/pdf', N'.\.\Files\f2f2b372-2401-45ff-85ce-acfe8f727ccc_podręcznik_algorytmy.pdf', 0),
    (10, N'WDI-WprowadzenieDoAlgorytmiki.pdf', N'application/pdf', N'.\.\Files\859cfaec-6cee-4982-9dd1-68fcf3d2bcab_WDI-WprowadzenieDoAlgorytmiki.pdf', 0),
    (11, N'ai_tech_Wpro_SI_W4_podstawy_ml.pdf', N'application/pdf', N'.\.\Files\b1b4ea71-29b7-4744-b763-928cdd1836ac_ai_tech_Wpro_SI_W4_podstawy_ml.pdf', 0),
    (12, N'AI_Wstep_14.pdf', N'application/pdf', N'.\.\Files\deb5a810-a304-4cd8-9855-a7123ce5d639_AI_Wstep_14.pdf', 0),
    (13, N'1.4.-M.-Gorustowicz-Efektywne-zarzadzanie-czasem-pracownika-–-wybrane-zagadnienia.pdf', N'application/pdf', N'.\.\Files\02cc5e56-7f71-442c-9d44-edb117f74556_1.4.-M.-Gorustowicz-Efektywne-zarzadzanie-czasem-pracownika-–-wybrane-zagadnienia.pdf', 0),
    (14, N'Olejniczak_EFEKTYWNE-ZARZADZANIE-CZASEM-WYBRANE-ZAGADNIENIA.pdf', N'application/pdf', N'.\.\Files\309e787c-d384-4e76-883c-55ac7ae7a14d_Olejniczak_EFEKTYWNE-ZARZADZANIE-CZASEM-WYBRANE-ZAGADNIENIA.pdf', 0),
    (15, N'gramatyka-niemiecka-A1-A2.pdf', N'application/pdf', N'.\.\Files\bba5771c-759f-4fdf-88fe-b1f281ac7771_gramatyka-niemiecka-A1-A2.pdf', 0),
    (16, N'niemiecki_slownictwo1_sample.pdf', N'application/pdf', N'.\.\Files\30a5ec5a-1033-4223-a0a1-9ca3da5b19a8_niemiecki_slownictwo1_sample.pdf', 0),
    (17, N'grafika.pdf', N'application/pdf', N'.\.\Files\77351534-c231-4bb1-b75d-ffd6737f98c1_grafika.pdf', 0),
    (18, N'Program_Analiza_danych_w_MS_Excel.pdf', N'application/pdf', N'.\.\Files\30337597-d06d-4e0b-a376-8c21ef38e713_Program_Analiza_danych_w_MS_Excel.pdf', 0),
    (19, N'analiza-i-prezentacja-danych-w-microsoft-excel-vademecum-walkenbacha-michael-alexander-john-walkenbach.pdf', N'application/pdf', N'.\.\Files\8ed1d92e-f8de-40d5-b922-0e013c3f7e22_analiza-i-prezentacja-danych-w-microsoft-excel-vademecum-walkenbacha-michael-alexander-john-walkenbach.pdf', 0),
    (20, N'react-w-dzialaniu-tworzenie-aplikacji-internetowych-stoyan-stefanov.pdf', N'application/pdf', N'.\.\Files\5f45a614-61f1-4649-a39a-ea0e4920813c_react-w-dzialaniu-tworzenie-aplikacji-internetowych-stoyan-stefanov.pdf', 0),
    (21, N'responsive-web-design-projektowanie-elastycznych-witryn-w-html5-i-css3-ben-frain.pdf', N'application/pdf', N'.\.\Files\0bdb6997-b19f-4f6f-b4fb-7a52a8c812a8_responsive-web-design-projektowanie-elastycznych-witryn-w-html5-i-css3-ben-frain.pdf', 0),
    (22, N'react-native-tworzenie-aplikacji-mobilnych-w-jezyku-javascript-wydanie-ii-bonnie-eisenman.pdf', N'application/pdf', N'.\.\Files\17059b32-10c4-45da-bd34-635e3c69666e_react-native-tworzenie-aplikacji-mobilnych-w-jezyku-javascript-wydanie-ii-bonnie-eisenman.pdf', 0),
    (23, N'sql.pdf', N'application/pdf', N'.\.\Files\4d4fa8f5-742e-4a29-9163-876950b6e749_sql.pdf', 0),
    (24, N'pobrane (1).jpg', N'image/jpeg', N'.\.\Files\ca5202ae-e441-470d-8ee2-e68a091a06be_pobrane (1).jpg', 0),
    (25, N'pobrane (2).jpg', N'image/jpeg', N'.\.\Files\ebe2b0c7-7657-4a3f-bec5-517815cc6cfa_pobrane (2).jpg', 0),
    (26, N'pobrane.png', N'image/png', N'.\.\Files\40c6a255-f1bd-4920-81e3-999e2da47a03_pobrane.png', 0),
    (27, N'8ohffle7awy01.webp', N'image/webp', N'.\.\Files\7bdf8c17-c402-4eca-8bde-38e2ac288c2b_8ohffle7awy01.webp', 0),
    (28, N'pobrane.jpg', N'image/jpeg', N'.\.\Files\0ff47cd1-0a91-4c67-94d2-c1001d72fafa_pobrane.jpg', 0),
    (29, N'Trzecia Konferencja Kół Naukowych w ramach Politechnicznej Sieci Via Carpatia im. Prezydenta RP Lecha Kaczyńskiego.pdf', N'application/pdf', N'.\.\Files\9aa3d91d-67fc-4a41-bc8a-5250f4774c59_Trzecia Konferencja Kół Naukowych w ramach Politechnicznej Sieci Via Carpatia im. Prezydenta RP Lecha Kaczyńskiego.pdf', 0);
SET IDENTITY_INSERT [Files] OFF;

-- FILE IN MATERIALS
INSERT INTO [FileInMaterials] 
    ([MaterialId], [FileId], [State]) 
VALUES 
    (14, 1, 0),
    (14, 2, 0),
    (14, 3, 0),
    (1, 4, 0),
    (2, 5, 0),
    (2, 6, 0),
    (3, 7, 0),
    (4, 8, 0),
    (4, 9, 0),
    (4, 10, 0),
    (5, 11, 0),
    (5, 12, 0),
    (6, 13, 0),
    (6, 14, 0),
    (7, 15, 0),
    (7, 16, 0),
    (8, 17, 0),
    (9, 18, 0),
    (9, 19, 0),
    (10, 20, 0),
    (11, 21, 0),
    (12, 22, 0),
    (13, 23, 0),
    (15, 24, 0),
    (15, 25, 0),
    (15, 26, 0),
    (15, 27, 0),
    (15, 28, 0),
    (17, 29, 0);

-- SCHOOLINGS
SET IDENTITY_INSERT [Schoolings] ON;
INSERT INTO [Schoolings] 
    ([Id], [CreatorId], [CategoryId], [Title], [Description], [Priority], [State]) 
VALUES 
    (1, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 1, N'Zaawansowane Techniki Programowania w Pythonie', N'Szkolenie dla doświadczonych programistów, które zagłębia się w zaawansowane techniki programowania w języku Python. Kurs obejmuje tematy takie jak optymalizacja kodu, wielowątkowość, programowanie asynchroniczne oraz wykorzystanie bibliotek do analizy danych.', 1, 0),
    (2, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 10, N'Podstawy Marketingu Cyfrowego', N'Szkolenie dla osób, które chcą zdobyć podstawową wiedzę na temat marketingu w internecie. Kurs obejmuje zagadnienia takie jak SEO, SEM, marketing w mediach społecznościowych i email marketing.', 2, 0),
    (3, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 10, N'Zarządzanie Projektami IT', N'Szkolenie przeznaczone dla osób chcących nauczyć się efektywnego zarządzania projektami IT. Kurs obejmuje metodologie Agile, Scrum oraz narzędzia do monitorowania postępu prac i zarządzania zespołami.', 3, 0),
    (4, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 7, N'Bezpieczeństwo w Internecie dla Firm', N'Szkolenie skierowane do pracowników firm, które uczą się o zagrożeniach w sieci i sposobach ich unikania. Kurs obejmuje tematy związane z zabezpieczaniem danych, ochroną przed atakami i politykami bezpieczeństwa.', 1, 0),
    (5, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 11, N'Zarządzanie Zasobami Ludzkimi w Przedsiębiorstwie', N'Szkolenie dla menedżerów HR, którzy chcą rozwijać swoje umiejętności w zakresie zarządzania pracownikami, rekrutacji, szkoleń oraz motywowania zespołów. Kurs omawia również aspekty prawne związane z zatrudnieniem.', 2, 0),
    (6, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 6, N'Podstawy UX/UI Design', N'Szkolenie dla osób, które chcą zdobyć wiedzę na temat projektowania doświadczeń użytkownika (UX) oraz interfejsów użytkownika (UI). Kurs dostarcza fundamentów w zakresie tworzenia przyjaznych i intuicyjnych interfejsów, które zapewniają użytkownikom pozytywne wrażenia.', 3, 0),
    (7, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 4, N'Zaawansowane Techniki Wyszukiwania i Analizy Danych', N'Szkolenie skierowane do analityków danych oraz specjalistów IT, którzy pragną poszerzyć swoje umiejętności w zakresie zaawansowanego przetwarzania danych. Program kursu obejmuje takie tematy jak analiza dużych zbiorów danych przy użyciu narzędzi takich jak Python i R, techniki wyszukiwania z wykorzystaniem baz danych SQL i NoSQL, a także tworzenie zaawansowanych algorytmów wyszukiwania i filtrowania informacji.', 2, 0),
    (8, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 10, N'Zarządzanie Ryzykiem w Firmie', N'Szkolenie przeznaczone dla menedżerów, którzy chcą zdobyć wiedzę na temat skutecznego zarządzania ryzykiem w organizacjach. Kurs obejmuje szeroki zakres tematów, takich jak identyfikacja i ocena ryzyka, metody minimalizacji ryzyk operacyjnych, finansowych i strategicznych, a także zarządzanie ryzykiem w projektach.', 3, 0),
    (9, N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', 1, N'Wprowadzenie do Programowania w JavaScript', N'Kurs skierowany do osób, które chcą nauczyć się podstaw programowania w jednym z najpopularniejszych języków na świecie – JavaScript. Szkolenie obejmuje podstawy składni języka, takie jak zmienne, pętle, funkcje, tablice oraz obiekty, a także bardziej zaawansowane tematy, takie jak manipulowanie DOM-em, asynchroniczność w JavaScript i używanie frameworków jak React.', 1, 0);
SET IDENTITY_INSERT [Schoolings] OFF;

-- SCHOOLING PARTS
SET IDENTITY_INSERT [SchoolingParts] ON
INSERT INTO [SchoolingParts] 
    ([Id], [SchoolingId], [Name], [Content], [State])
VALUES 
    (1, 1, N'Wprowadzenie do Programowania Obiektowego w Pythonie', N'W tej części kursu uczestnicy zapoznają się z podstawami programowania obiektowego w Pythonie. Omówimy klasy, obiekty, dziedziczenie oraz polimorfizm. Kurs pokaże, jak organizować kod w sposób modularny, tworzyć hierarchie klas i korzystać z właściwości obiektów. Uczestnicy nauczą się również jak za pomocą Python implementować i wykorzystywać wzorce projektowe, co umożliwia tworzenie bardziej zaawansowanych aplikacji.', 0),
    (2, 1, N'Optymalizacja i Wydajność Pythona', N'W tej części kursu uczestnicy poznają techniki optymalizacji kodu w Pythonie, w tym zarządzanie pamięcią, profilowanie aplikacji i używanie bibliotek do przyspieszania obliczeń, takich jak NumPy. Omówimy również zasady dobrego kodowania, które pozwolą na pisanie bardziej wydajnych i skalowalnych aplikacji. Celem jest zrozumienie, jak uniknąć typowych pułapek w Pythonie, które mogą wpływać na wydajność aplikacji.', 0),
    (3, 2, N'SEO i SEM – Wprowadzenie do Optymalizacji', N'W tej części kursu omówimy podstawy SEO (Search Engine Optimization) oraz SEM (Search Engine Marketing). Uczestnicy dowiedzą się, jak poprawnie zoptymalizować stronę internetową, aby zwiększyć jej widoczność w wyszukiwarkach internetowych. Omówimy także narzędzia i strategie reklamowe, takie jak Google Ads, które umożliwiają efektywne pozycjonowanie stron oraz generowanie ruchu na stronie. Celem jest zrozumienie podstawowych zasad marketingu w wyszukiwarkach oraz sposobów na optymalizację treści pod kątem algorytmów Google.', 0),
    (4, 2, N'Media Społecznościowe i E-mail Marketing', N'Celem tej części kursu jest zapoznanie uczestników z podstawami marketingu na mediach społecznościowych oraz strategii e-mail marketingowej. Kurs obejmuje tematykę tworzenia skutecznych kampanii reklamowych na Facebooku, Instagramie, LinkedIn oraz Twitterze. Dodatkowo uczestnicy dowiedzą się, jak budować listy mailingowe, tworzyć wartościowe treści do wysyłki oraz jak analizować efektywność kampanii e-mailowych za pomocą narzędzi takich jak MailChimp.', 0),
    (5, 3, N'Planowanie Projektu IT', N'W tej części kursu uczestnicy nauczą się, jak efektywnie planować projekty IT. Omówimy tworzenie harmonogramów, przypisywanie zasobów do zadań, analizę ryzyka oraz ustalanie budżetów. Uczestnicy dowiedzą się, jak przygotować kompleksowy plan projektu, który będzie stanowił fundament do skutecznej realizacji projektu IT. Kurs zawiera również omówienie metodologii takich jak Agile i Waterfall oraz ich zastosowanie w praktyce.', 0),
    (6, 3, N'Monitorowanie i Raportowanie Postępu Projektu', N'Ta część kursu koncentruje się na technikach monitorowania i raportowania postępu projektu IT. Uczestnicy dowiedzą się, jak używać narzędzi do zarządzania projektami (np. Jira, Asana) do śledzenia postępu, zarządzania zadaniami i komunikowania się z zespołem. Kurs pokaże, jak przygotować raporty postępu, identyfikować potencjalne problemy oraz jak dokonywać korekt w planie projektowym, aby projekt przebiegał zgodnie z harmonogramem.', 0),
    (7, 4, N'Ochrona Przed Cyberzagrożeniami', N'W tej części kursu uczestnicy zapoznają się z zagrożeniami związanymi z cyberprzestępczością i dowiedzą się, jak chronić firmowe systemy przed atakami. Omówimy rodzaje cyberataków, takie jak phishing, ransomware, ataki DDoS oraz zagrożenia związane z oprogramowaniem złośliwym. Kurs skupi się na metodach ochrony przed tymi zagrożeniami, takich jak używanie zapór sieciowych, oprogramowania antywirusowego oraz szyfrowanie danych.', 0),
    (8, 4, N'Ochrona Danych i Zgodność z RODO', N'Celem tej części kursu jest zapoznanie uczestników z podstawowymi zasadami ochrony danych osobowych oraz wymogami wynikającymi z rozporządzenia RODO. Omówimy, jak wdrożyć polityki ochrony danych w firmie, jak zarządzać dostępem do informacji oraz jak przechowywać dane w sposób zgodny z przepisami prawa. Uczestnicy dowiedzą się, jak przeprowadzać audyty zgodności z RODO i jakie procedury należy wdrożyć, aby zapewnić bezpieczeństwo danych osobowych.', 0),
    (9, 5, N'Rekrutacja i Selekcja Pracowników', N'W tej części kursu uczestnicy nauczą się skutecznych metod rekrutacji i selekcji pracowników. Omówimy proces tworzenia ogłoszeń rekrutacyjnych, przeprowadzania rozmów kwalifikacyjnych oraz oceniania kompetencji kandydatów. Kurs skupi się również na narzędziach i technikach, które pomagają w znalezieniu odpowiednich osób na określone stanowiska w firmie. Uczestnicy dowiedzą się, jak zwiększyć efektywność procesu rekrutacji, minimalizując ryzyko błędów w selekcji.', 0),
    (10, 5, N'Motywowanie i Rozwój Pracowników', N'Ta część kursu koncentruje się na technikach motywowania pracowników oraz strategiach wspierania ich rozwoju zawodowego. Omówimy metody oceny efektywności pracy, jak tworzyć plany rozwoju kariery oraz jak wdrażać programy szkoleń i mentoringu. Kurs pokaże, jak skutecznie motywować zespół, aby osiągać lepsze wyniki i zwiększać zaangażowanie pracowników w życie firmy.', 0),
    (11, 6, N'Badania UX i Testowanie Użyteczności', N'W tej części kursu uczestnicy nauczą się, jak przeprowadzać badania użytkowników, testowanie użyteczności oraz analizować wyniki. Omówimy metodyki takie jak testy A/B, wywiady z użytkownikami, mapowanie ścieżek użytkownika i analiza danych. Uczestnicy dowiedzą się, jak stosować te techniki w praktyce, aby poprawić doświadczenie użytkowników w aplikacjach mobilnych oraz stronach internetowych.', 0),
    (12, 6, N'Projektowanie Interfejsów – Podstawy UI', N'Część ta koncentruje się na projektowaniu interfejsów użytkownika (UI), w tym na tworzeniu estetycznych i intuicyjnych układów graficznych. Kurs obejmuje zagadnienia związane z typografią, kolorystyką, rozmieszczeniem elementów oraz tworzeniem responsywnych interfejsów, które dobrze wyglądają na różnych urządzeniach. Uczestnicy poznają również narzędzia takie jak Adobe XD, Figma czy Sketch, które pomagają w projektowaniu interfejsów.', 0);
SET IDENTITY_INSERT [SchoolingParts] OFF

-- MATERIALS SCHOOLING PARTS
INSERT INTO [MaterialsSchoolingParts] 
    ([MaterialsId], [SchoolingPartId], [State])
VALUES 
    (1, 1, 0),
    (3, 3, 0),
    (4, 2, 0),
    (6, 5, 0),
    (8, 11, 0),
    (9, 6, 0),
    (9, 9, 0),
    (10, 12, 0),
    (11, 11, 0),
    (12, 12, 0),
    (14, 7, 0),
    (17, 11, 0);


-- SCHOOLING USERS
INSERT INTO [SchoolingsUsers] 
    ([NewbieId], [SchoolingId], [State])
VALUES 
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', 1, 0),
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', 2, 0),
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', 3, 0),
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', 4, 0),
    (N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', 5, 0),
    (N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', 6, 0),
    (N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', 7, 0),
    (N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', 8, 0),
    (N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', 9, 0);


-- TASK CONTENTS
SET IDENTITY_INSERT [TaskContents] ON
INSERT INTO [TaskContents] 
    ([Id], [CreatorId], [CategoryId], [MaterialsId], [Title], [Description])
VALUES 
    (1, '437d2b01-ed58-4fcf-931a-08dd3251ce0a', 1, 1, N'Podstawy Pythona', N'Wprowadzenie do podstaw programowania w Pythonie.'),
    (2, '437d2b01-ed58-4fcf-931a-08dd3251ce0a', 4, 9, N'Analiza danych w Excelu', N'Podstawy analizy danych z użyciem Excela.'),
    (3, '437d2b01-ed58-4fcf-931a-08dd3251ce0a', 5, 17, N'Testowanie Oprogramowania', N'Wprowadzenie do testowania oprogramowania.'),
    (4, '30943099-da50-4271-931c-08dd3251ce0a', 2, 2, N'Zasady UX', N'Zrozumienie kluczowych zasad projektowania UX.'),
    (5, '30943099-da50-4271-931c-08dd3251ce0a', 2, 8, N'Grafika komputerowa', N'Podstawy grafiki komputerowej.'),
    (6, '30943099-da50-4271-931c-08dd3251ce0a', 3, 3, N'Podstawy Marketingu', N'Podstawowe techniki marketingu internetowego.'),
    (7, '30943099-da50-4271-931c-08dd3251ce0a', 3, 14, N'Bezpieczeństwo w Sieci', N'Ochrona przed cyberzagrożeniami.'),
    (8, '30943099-da50-4271-931c-08dd3251ce0a', 5, 17, N'Testowanie Oprogramowania', N'Zaawansowane testowanie oprogramowania.'),
    (9, '30943099-da50-4271-931c-08dd3251ce0a', 4, 9, N'Analiza Danych', N'Zaawansowana analiza danych w Excelu.');
SET IDENTITY_INSERT [TaskContents] OFF

-- PRESETS
SET IDENTITY_INSERT [Presets] ON
INSERT INTO [Presets] 
    ([Id], [CreatorId], [Name])
VALUES 
    (1, '30943099-da50-4271-931c-08dd3251ce0a', N'Training UX & Design'),
    (2, '30943099-da50-4271-931c-08dd3251ce0a', N'Training Marketing');
SET IDENTITY_INSERT [Presets] OFF

-- TASK PRESETS
INSERT INTO [TaskPresets] 
    ([PresetId], [TaskContentId])
VALUES 
    (1, 2),
    (1, 3),
    (1, 4),
    (2, 5),
    (2, 6),
    (2, 7);

-- TASKS
SET IDENTITY_INSERT [Tasks] ON
INSERT INTO [Tasks] 
    ([ID],[NewbieId], [AssigningId], [TaskContentId], [Status], [AssignmentDate],[FINALIZATIONDATE], [DEADLINE],[SPENDTIME], [Priority], [State],[ROADMAPPOINTID],[RATE]) 
VALUES
    (1, '7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '30943099-da50-4271-931c-08dd3251ce0a', 1, 0, GETDATE(),NULL, NULL, 0, 1, 0, NULL, NULL),
    (2, 'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '45678901-2345-6789-0abc-def123456789', 2, 0, GETDATE(),NULL, NULL, 0, 2, 0, NULL, NULL),
    (3, 'abcdef01-2345-6789-0123-456789abcdef', '78901234-5678-9012-cdef-0123456789ab', 3, 0, GETDATE(),NULL, NULL, 0, 3, 0, NULL, NULL),
    (4, '34567890-1234-5678-90ab-cdef01234567', '30943099-da50-4271-931c-08dd3251ce0a', 4, 0, GETDATE(),NULL, NULL, 0, 1, 0, NULL, NULL),
    (5, '67890123-4567-8901-bcde-f0123456789a', '45678901-2345-6789-0abc-def123456789', 5, 0, GETDATE(),NULL, NULL, 0, 2, 0, NULL, NULL),
    (6, '7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '30943099-da50-4271-931c-08dd3251ce0a', 1, 0, '2025-01-01', NULL, '2025-01-20', 0, 1, 0, NULL, NULL),
    (7, 'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '45678901-2345-6789-0abc-def123456789', 2, 0, '2025-01-02', NULL, '2025-01-21', 0, 2, 0, NULL, NULL),
    (8, 'abcdef01-2345-6789-0123-456789abcdef', '78901234-5678-9012-cdef-0123456789ab', 3, 10, '2025-01-03', NULL, '2025-01-22', 3.5, 3, 0, NULL, NULL),
    (9, '34567890-1234-5678-90ab-cdef01234567', '30943099-da50-4271-931c-08dd3251ce0a', 4, 10, '2025-01-04', NULL, '2025-01-23', 2.0, 1, 0, NULL, NULL),
    (10,'67890123-4567-8901-bcde-f0123456789a', '45678901-2345-6789-0abc-def123456789', 5, 20, '2025-01-05', NULL, '2025-01-24', 5.0, 2, 0, NULL, NULL),
    (11,'abcdef01-2345-6789-0123-456789abcdef', '78901234-5678-9012-cdef-0123456789ab', 3, 20, '2025-01-06', NULL, '2025-01-25', 4.0, 3, 0, NULL, NULL),
    (12,'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '30943099-da50-4271-931c-08dd3251ce0a', 1, 30, '2025-01-07', NULL, '2025-01-26', 6.0, 1, 0, NULL, NULL),
    (13,'d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '45678901-2345-6789-0abc-def123456789', 2, 30, '2025-01-08', NULL, '2025-01-27', 7.5, 2, 0, NULL, NULL),
    (14,'67890123-4567-8901-bcde-f0123456789a', '45678901-2345-6789-0abc-def123456789', 5, 40, '2025-01-09', '2025-01-15', '2025-01-28', 8.0, 2, 0, NULL, NULL),
    (15,'abcdef01-2345-6789-0123-456789abcdef', '78901234-5678-9012-cdef-0123456789ab', 3, 40, '2025-01-10', '2025-01-16', '2025-01-29', 9.5, 3, 0, NULL, NULL),
    (16,'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '30943099-da50-4271-931c-08dd3251ce0a', 6, 0, GETDATE(), NULL, '2025-01-30', 0, 1, 0, NULL, NULL),
    (17,'d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '45678901-2345-6789-0abc-def123456789', 7, 0, GETDATE(), NULL, '2025-01-31', 0,2, 0, NULL, NULL),
    (18,'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '78901234-5678-9012-cdef-0123456789ab', 8, 10, GETDATE(), NULL, '2025-02-01', 0,3, 0, NULL, NULL),
    (19,'67890123-4567-8901-bcde-f0123456789a', '30943099-da50-4271-931c-08dd3251ce0a', 9, 10, GETDATE(), NULL, '2025-02-02', 0,1, 0, NULL, NULL),
    (20,'abcdef01-2345-6789-0123-456789abcdef', '45678901-2345-6789-0abc-def123456789', 1, 20, GETDATE(), NULL, '2025-02-03', 0,2, 0, NULL, NULL),
    (21,'34567890-1234-5678-90ab-cdef01234567', '78901234-5678-9012-cdef-0123456789ab', 2, 20, GETDATE(), NULL, '2025-02-04', 0,3, 0, NULL, NULL),
    (22,'67890123-4567-8901-bcde-f0123456789a', '30943099-da50-4271-931c-08dd3251ce0a', 4, 30, GETDATE(), NULL, '2025-02-05', 0,1, 0, NULL, NULL),
    (23, '7fd8c6b5-9199-49b5-931d-08dd3251ce0a', '45678901-2345-6789-0abc-def123456789', 4, 30, GETDATE(), NULL, '2025-02-06', 0,2, 0, NULL, NULL),
    (24, 'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '78901234-5678-9012-cdef-0123456789ab', 5, 40, GETDATE(), '2025-01-10', '2025-02-07', 2, 3, 0, NULL, NULL),
    (25, 'abcdef01-2345-6789-0123-456789abcdef', '30943099-da50-4271-931c-08dd3251ce0a', 6, 40, GETDATE(), '2025-01-11', '2025-02-08', 3, 1, 0, NULL, NULL),
    (26, '34567890-1234-5678-90ab-cdef01234567', '45678901-2345-6789-0abc-def123456789', 7, 40, GETDATE(), '2025-01-12', '2025-02-09', 4,2, 0, NULL, NULL),
    (27, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '78901234-5678-9012-cdef-0123456789ab', 8, 40, GETDATE(), '2025-01-13', '2025-02-10', 2,3, 0, NULL, NULL),
    (28, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 1, 0, GETDATE(),NULL, NULL, 0, 1, 0, NULL, NULL),
    (29, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 2, 0, GETDATE(),NULL, NULL, 0, 2, 0, NULL, NULL),
    (30, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '78901234-5678-9012-cdef-0123456789ab', 3, 0, GETDATE(),NULL, NULL, 0, 3, 0, NULL, NULL),
    (31, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 4, 0, GETDATE(),NULL, NULL, 0, 1, 0, NULL, NULL),
    (32, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 5, 0, GETDATE(),NULL, NULL, 0, 2, 0, NULL, NULL),
    (33, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 1, 0, '2025-01-01', NULL, '2025-01-20', 0, 1, 0, NULL, NULL),
    (34, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 2, 0, '2025-01-02', NULL, '2025-01-21', 0, 2, 0, NULL, NULL),
    (35, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '78901234-5678-9012-cdef-0123456789ab', 3, 10, '2025-01-03', NULL, '2025-01-22', 3.5, 3, 0, NULL, NULL),
    (36, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 4, 10, '2025-01-04', NULL, '2025-01-23', 2.0, 1, 0, NULL, NULL),
    (37, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 5, 20, '2025-01-05', NULL, '2025-01-24', 5.0, 2, 0, NULL, NULL),
    (38, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '78901234-5678-9012-cdef-0123456789ab', 3, 20, '2025-01-06', NULL, '2025-01-25', 4.0, 3, 0, NULL, NULL),
    (39, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 1, 30, '2025-01-07', NULL, '2025-01-26', 6.0, 1, 0, NULL, NULL),
    (40, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 2, 30, '2025-01-08', NULL, '2025-01-27', 7.5, 2, 0, NULL, NULL),
    (41, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '45678901-2345-6789-0abc-def123456789', 5, 40, '2025-01-09', '2025-01-15', '2025-01-28', 8.0, 2, 0, NULL, NULL),
    (42, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', '78901234-5678-9012-cdef-0123456789ab', 3, 40, '2025-01-10', '2025-01-16', '2025-01-29', 9.5, 3, 0, NULL, NULL);
SET IDENTITY_INSERT [Tasks] OFF

-- NOTIFICATIONS
SET IDENTITY_INSERT [Notifications] ON
INSERT INTO [Notifications] 
    (Id, SenderId, Title, Message, SendDate, Source, State) 
VALUES 
    (2, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'New Assignment!', 'Prepare report', '2025-01-15 09:23:45', '/Tasks/27', 0),
    (3, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Alert', 'Review documents', '2025-01-20 14:15:22', '/Tasks/28', 0),
    (4, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Urgent Task', 'Call client', '2025-01-25 11:47:13', '/Tasks/29', 0),
    (5, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'New Job', 'Update database', '2025-02-01 16:30:59', '/Tasks/30', 0),
    (6, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Reminder', 'Send email', '2025-02-05 13:12:34', '/Tasks/31', 0),
    (7, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Action Required', 'Check stats', '2025-02-10 10:25:47', '/Tasks/32', 0),
    (8, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'New Task', 'Fix bug', '2025-02-15 15:55:11', '/Tasks/33', 0),
    (9, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Quick Task', 'Upload file', '2025-02-20 09:41:28', '/Tasks/34', 0),
    (10, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Update', 'Test feature', '2025-02-25 12:33:19', '/Tasks/35', 0),
    (11, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'New Duty', 'Write code', '2025-03-01 14:22:56', '/Tasks/36', 0),
    (12, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Notice', 'Meet team', '2025-03-05 17:18:43', '/Tasks/37', 0),
    (13, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Work Alert', 'Plan sprint', '2025-03-10 08:59:32', '/Tasks/38', 0),
    (14, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Added', 'Design UI', '2025-03-15 11:44:15', '/Tasks/39', 0),
    (15, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'New Item', 'Check logs', '2025-03-20 13:27:58', '/Tasks/40', 0),
    (16, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Task Now', 'Deploy app', '2025-03-22 16:09:41', '/Tasks/41', 0),
    (17, '04D68924-791D-4022-F2E3-08DD33FC8FD5', 'Urgent!', 'Fix error', '2025-03-23 01:06:24', '/Tasks/42', 0);
SET IDENTITY_INSERT [Notifications] OFF

-- USERS NOTIFICATIONS
INSERT INTO [UsersNotifications] 
    (NotificationId, ReceiverId, State, IsRead) 
VALUES 
    (2, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (3, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (4, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (5, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (6, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (7, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (8, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (9, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (10, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (11, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (12, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (13, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (14, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (15, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (16, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0),
    (17, '555843BB-B38F-4387-F2E2-08DD33FC8FD5', 0, 0);

-- FEEDBACKS
SET IDENTITY_INSERT [Feedbacks] ON
INSERT INTO [Feedbacks] 
    ([Id], [SenderId], [ReceiverId], [Title], [Description], [ResourceType], [ResourceId], [IsResolved], [CreatedDate], [State])
VALUES 
    (1, N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Dobra robota', N'Szkolenie było bardzo pomocne i szczegółowe.', 20, 2, 1, '2025-01-13T18:56:13', 0),
    (2, N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Przydatny materiał', N'Materiał zawierał wszystkie potrzebne informacje.', 20, 3, 0, '2025-01-13T18:57:13', 0),
    (3, N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Doskonałe szkolenie', N'To szkolenie było bardzo pomocne dla całego zespołu.', 20, 6, 0, '2025-01-13T19:00:13', 0),
    (4, N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Świetny kurs', N'Szkolenie dostarczyło pełnych i szczegółowych informacji.', 20, 9, 0, '2025-01-13T19:03:13', 0),
    (5, N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Kompletny materiał', N'Szkolenie pokrywa wszystkie niezbędne aspekty tematu.', 20, 2, 0, '2025-01-13T19:06:13', 0),
    (6, N'7fd8c6b5-9199-49b5-931d-08dd3251ce0a', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Inspirujące szkolenie', N'Materiał był bardzo inspirujący i dobrze opracowany.', 20, 5, 0, '2025-01-13T19:09:13', 0),
    (7, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Niedokładne wyjaśnienie', N'Temat został przedstawiony, ale brakowało szczegółów.', 30, 1, 0, '2025-01-13T18:55:13', 0),
    (8, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Wysoka jakość', N'Odpowiedź przedstawiona została na bardzo wysokim poziomie.', 30, 4, 0, '2025-01-13T18:58:13', 0),
    (9, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Niewystarczające szczegóły', N'FAQ powinno zawierać więcej przykładów praktycznych.', 30, 7, 0, '2025-01-13T19:01:13', 0),
    (10, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'Niejasne odpowiedzi', N'Niektóre odpowiedzi w FAQ są nieprecyzyjne.', 30, 10, 0, '2025-01-13T19:04:13', 0),
    (11, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'FAQ do poprawy', N'Kilka pytań w FAQ jest nieaktualnych lub mylących.', 30, 13, 0, '2025-01-13T19:07:13', 0),
    (12, N'555843BB-B38F-4387-F2E2-08DD33FC8FD5', N'437d2b01-ed58-4fcf-931a-08dd3251ce0a', N'FAQ pomocne', N'FAQ było pomocne, ale brakuje bardziej zaawansowanych tematów.', 30, 16, 0, '2025-01-13T19:10:13', 0),
    (13, N'abcdef01-2345-6789-0123-456789abcdef', N'78901234-5678-9012-cdef-0123456789ab', N'Brak przykładów', N'Brakuje konkretnych przykładów w treści zadania.', 40, 8, 0, '2025-01-13T19:02:13', 0),
    (14, N'abcdef01-2345-6789-0123-456789abcdef', N'78901234-5678-9012-cdef-0123456789ab', N'Dobry start', N'Task dobrze wprowadza do tematu, ale wymaga rozbudowy.', 40, 11, 0, '2025-01-13T19:05:13', 0),
    (15, N'67890123-4567-8901-bcde-f0123456789a', N'45678901-2345-6789-0abc-def123456789', N'Niekompletne informacje', N'Task powinien zawierać więcej szczegółowych wskazówek.', 40, 14, 0, '2025-01-13T19:08:13', 0),
    (16, N'd5636dcb-3d4f-4e2d-931e-08dd3251ce0a', N'45678901-2345-6789-0abc-def123456789', N'Zadanie z wyzwaniem', N'Task był wymagający, ale dobrze uczył praktyki.', 40, 17, 0, '2025-01-13T19:11:13', 0);
SET IDENTITY_INSERT [Feedbacks] OFF

-- NEWBIE MENTORS
INSERT INTO [catch-up-db].[dbo].[NewbiesMentors] 
    ([NewbieId], [MentorId], [State], [EndDate], [StartDate]) 
VALUES
    -- Newbie 555843BB-B38F-4387-F2E2-08DD33FC8FD5
    ('555843BB-B38F-4387-F2E2-08DD33FC8FD5', '04D68924-791D-4022-F2E3-08DD33FC8FD5', 0, NULL, GETDATE()),
    ('555843BB-B38F-4387-F2E2-08DD33FC8FD5', '30943099-da50-4271-931c-08dd3251ce0a', 0, NULL, GETDATE()),
    ('555843BB-B38F-4387-F2E2-08DD33FC8FD5', '12a34bcd-56ef-7890-1234-56789abcdef0', 0, NULL, GETDATE()),
    -- Newbie d5636dcb-3d4f-4e2d-931e-08dd3251ce0a
    ('d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '04D68924-791D-4022-F2E3-08DD33FC8FD5', 0, NULL, GETDATE()),
    ('d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '90123456-7890-1234-ef01-23456789abcd', 0, NULL, GETDATE()),
    ('d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '23456789-0123-4567-1234-56789abcdef0', 0, NULL, GETDATE()),
    ('d5636dcb-3d4f-4e2d-931e-08dd3251ce0a', '56789012-3456-7890-4567-89abcdef0123', 0, NULL, GETDATE()),
    -- Newbie abcdef01-2345-6789-0123-456789abcdef
    ('abcdef01-2345-6789-0123-456789abcdef', '04D68924-791D-4022-F2E3-08DD33FC8FD5', 0, NULL, GETDATE()),
    -- Newbie 34567890-1234-5678-90ab-cdef01234567
    ('34567890-1234-5678-90ab-cdef01234567', '04D68924-791D-4022-F2E3-08DD33FC8FD5', 0, NULL, GETDATE()),
    ('34567890-1234-5678-90ab-cdef01234567', '30943099-da50-4271-931c-08dd3251ce0a', 0, NULL, GETDATE()),
    ('34567890-1234-5678-90ab-cdef01234567', '78901234-5678-9012-cdef-0123456789ab', 0, NULL, GETDATE()),
    ('34567890-1234-5678-90ab-cdef01234567', '90123456-7890-1234-ef01-23456789abcd', 0, NULL, GETDATE()),
    ('34567890-1234-5678-90ab-cdef01234567', '56789012-3456-7890-4567-89abcdef0123', 0, NULL, GETDATE()),
    -- Newbie 67890123-4567-8901-bcde-f0123456789a
    ('67890123-4567-8901-bcde-f0123456789a', '45678901-2345-6789-0abc-def123456789', 0, NULL, GETDATE()),
    ('67890123-4567-8901-bcde-f0123456789a', '78901234-5678-9012-cdef-0123456789ab', 0, NULL, GETDATE()),
    ('67890123-4567-8901-bcde-f0123456789a', '90123456-7890-1234-ef01-23456789abcd', 0, NULL, GETDATE()),
    ('67890123-4567-8901-bcde-f0123456789a', '23456789-0123-4567-1234-56789abcdef0', 0, NULL, GETDATE()),
    ('67890123-4567-8901-bcde-f0123456789a', '56789012-3456-7890-4567-89abcdef0123', 0, NULL, GETDATE()),
    -- Newbie 89012345-6789-0123-def0-123456789abc
    ('89012345-6789-0123-def0-123456789abc', '04D68924-791D-4022-F2E3-08DD33FC8FD5', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '30943099-da50-4271-931c-08dd3251ce0a', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '12a34bcd-56ef-7890-1234-56789abcdef0', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '45678901-2345-6789-0abc-def123456789', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '78901234-5678-9012-cdef-0123456789ab', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '90123456-7890-1234-ef01-23456789abcd', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '23456789-0123-4567-1234-56789abcdef0', 0, NULL, GETDATE()),
    ('89012345-6789-0123-def0-123456789abc', '56789012-3456-7890-4567-89abcdef0123', 0, NULL, GETDATE()),
    -- Newbie 34567890-1234-5678-2345-6789abcdef01
    ('34567890-1234-5678-2345-6789abcdef01', '23456789-0123-4567-1234-56789abcdef0', 0, NULL, GETDATE()),
    ('34567890-1234-5678-2345-6789abcdef01', '56789012-3456-7890-4567-89abcdef0123', 0, NULL, GETDATE());