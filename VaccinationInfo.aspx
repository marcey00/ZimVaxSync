<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VaccinationInfo.aspx.cs" Inherits="ZimVaxSync.VaccinationInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vaccination Information</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
           background-image: url('background.png');
            padding: 30px;
            color: #2c3e50;
        }

        h2, h3 {
            color: #2e6da4;
            text-align: center;
        }

        .info-container {
            background-color: #ffffff;
            padding: 25px;
            margin-bottom: 40px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.05);
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            border: 1px solid #000;
            padding: 10px;
            font-size: 14px;
            vertical-align: top;
            text-align: left;
        }

        th {
            background-color: #d9edf7;
            color: #31708f;
        }

        .back-button {
            display: inline-block;
            padding: 12px 24px;
            background-color: #337ab7;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 30px;
        }

        .back-button:hover {
            background-color: #286090;
        }

        ul {
            margin-top: 10px;
            padding-left: 20px;
        }

        ul li {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="info-container">
            <h2>CHILDHOOD VACCINATION AND CARE INFORMATION</h2>

            <table>
                <thead>
                    <tr>
                        <th>Vaccine</th>
                        <th>Disease Prevented</th>
                        <th>Chirwere Chinodzivirirwa (Shona)</th>
                        <th>Umkhuhlane Ovikelwayo (Ndebele)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Hep B</td>
                        <td>Hepatitis</td>
                        <td>Gomarara rechiropa</td>
                        <td>Imvukuzane yesibindi</td>
                    </tr>
                    <tr>
                        <td>Pentavalent</td>
                        <td>Diphtheria, Pertussis, Tetanus, Meningitis, Influenza, cancer of the Liver</td>
                        <td>Rukanda pahuro, Chiomashaya, chipembwe, Meningitis, Fruenza, Gomarara rechiropa</td>
                        <td>Amalonda amakhulu, Amaketane, Uphepha oludondoyo, Meningitis, Umvimbano olegazi, Imvukuzane yesibindi</td>
                    </tr>
                    <tr>
                        <td>OPV, IPV</td>
                        <td>Poliomyelitis</td>
                        <td>Mheta makumbo</td>
                        <td>Imbeleko</td>
                    </tr>
                    <tr>
                        <td>PCV13</td>
                        <td>Pneumonia</td>
                        <td>Mabayo</td>
                        <td>Isihlabo</td>
                    </tr>
                    <tr>
                        <td>Rotavirus</td>
                        <td>Rotavirus , Diarrhoea</td>
                        <td>Manyoka</td>
                        <td>Isihudo</td>
                    </tr>
                    <tr>
                        <td>Measles, Rubella</td>
                        <td>Measles , Rubella</td>
                        <td>Gwirikwiti</td>
                        <td>Indingindi</td>
                    </tr>
                    <tr>
                        <td>Td</td>
                        <td>(Tetanus and Diphtheria)</td>
                        <td>(Chiomesa shaya, Rukanda pahuro)</td>
                        <td>Amakethane lophephe oludonsayo</td>
                    </tr>
                    <tr>
                        <td>HPV</td>
                        <td>Cancer of the Cervix</td>
                        <td>Gomarara yemuromo wechibereko</td>
                        <td>Imvukuzane yomlomo wesibeletho</td>
                    </tr>
                    <tr>
                        <td>Vitamin A</td>
                        <td>Night Blindness</td>
                        <td>Kusaona zvakanaka murima</td>
                        <td>Ukungaboni kahle emnyabeni</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="info-container">
            <h3>GOOD INFANT FEEDING PRACTICE</h3>
            <p>Give breast milk only for the first 6 months. Introduce solids and liquids from 6 months. Continue breastfeeding up to 24 months or beyond unless advised otherwise by a health worker.</p>
            <p><strong>SHONA:</strong> Ipai mwana mukaka wezamu chete pamwedzi mitanhatu yekutanga. Ipai kumwe kudya kana kunwa kubva pamwedzi mitanhatu. Rambai muchiyamwisa kusvika pamakore maviri kana kudarika kunze kwekuti makurukura neveutano.</p>
            <p><strong>NDEBELE:</strong> Nika umntwana uchago lwebele lodwa kunyanga eziyisithupha zokuqala zokuzalwa. Qalisa ukunika umntwana okunye ukudla lokunathwayo nxa eselenyamga eziyisithupha. Qhubeka umunyisa umntwana aze abe leminyaka emibili kusiyaphambili, ngaphandle ukwayiswe ngabezempilakahle.</p>

            <h3>SALT AND SUGAR SOLUTION (SSS)</h3>
            <p>Give this SOLUTION as often as possible in case of DIARRHOEA and continue feeding and breast feeding. Consult the community health worker in your area. Take your child to the health facility for further management.</p>
            <p><strong>SHONA:</strong> Ipai mwana MVURA iyi nguva dzose kana ane MANYOKA moramba muchimupa zvokudya nekumuyamwisa. Onai vehutano vemunharaunda menyu, endesai mwana uyu kuchipatara anoongororwa.</p>
            <p><strong>NDEBELE:</strong> Nika umntwana AMANZI ale SAWUDO LETSHUKELA ngasikhati sonke EHUDO, njalo qhubeka unike umntwana ukudla lokumunyisa. Bona abezempilakahle abasesigabeni sakho ngokuphangisa. Hambisa umntwana esibhedlela ayehlolwa.</p>
        </div>

        <div class="info-container">
            <h2>OTHER ESSENTIAL VACCINES AND INFORMATION</h2>
            <ul>
                <li><strong>COVID-19:</strong> Protects against severe illness and death from coronavirus. Multiple doses or boosters may be required depending on vaccine type and age group.</li>
                <li><strong>Hepatitis A:</strong> Prevents acute liver disease caused by the Hep A virus. Typically recommended for children and travelers.</li>
                <li><strong>Hepatitis B:</strong> Protects against a viral infection that attacks the liver. Included in the Penta vaccine for infants; adults may also require boosters.</li>
                <li><strong>Typhoid:</strong> Prevents typhoid fever caused by contaminated food and water. Recommended especially in outbreak-prone areas.</li>
                <li><strong>Rabies:</strong> A critical vaccine after animal bites and for high-risk occupations. Should be administered promptly after exposure.</li>
                <li><strong>Chikungunya:</strong> Protects against a mosquito-borne viral disease. Important in regions with recent outbreaks.</li>
                <li><strong>Yellow Fever:</strong> Mandatory for travelers to and from certain regions. Offers lifelong protection after a single dose.</li>
                <li><strong>Cholera:</strong> Helps prevent cholera transmission during outbreaks. Often provided in two doses orally.</li>
                <li><strong>Meningitis:</strong> Prevents life-threatening infection of the brain and spinal cord. Given as part of routine immunization or during epidemics.</li>
            </ul>
            <p>Always consult a qualified health provider or clinic for vaccination schedules, eligibility, and updates regarding availability of these vaccines.</p>
        </div>

        <a href="CaregiverDashboard.aspx" class="back-button">&larr; Back to Dashboard</a>
    </form>
</body>
</html>
