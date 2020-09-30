using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Xml;

using FluentAssertions;

using NodaTime;

using Tiesmaster.ProjectZen.Domain.Bag;

using Xunit;

namespace Tiesmaster.ProjectZen.BagImporter.UnitTests
{
    public class BagParserTests
    {
        [Fact]
        public void ParsePanden()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Pand>
          <bag_LVC:identificatie>0599100000661089</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:pandGeometrie>
            <gml:Polygon srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:exterior>
                <gml:LinearRing>
                  <gml:posList srsDimension=""3"" count=""71"">91810.39 435310.8 0.0 91811.73 435310.76 0.0 91811.741 435311.15 0.0 91811.75 435311.47 0.0 91811.691 435312.149 0.0 91811.522 435312.809 0.0 91811.248 435313.433 0.0 91810.876 435314.003 0.0 91810.416 435314.506 0.0 91809.88 435314.927 0.0 91809.283 435315.256 0.0 91808.641 435315.482 0.0 91807.97 435315.602 0.0 91807.61 435315.62 0.0 91807.63 435314.27 0.0 91798.96 435314.3 0.0 91798.98 435314.97 0.0 91798.794 435315.83 0.0 91798.645 435316.101 0.0 91798.457 435316.346 0.0 91798.235 435316.56 0.0 91797.983 435316.739 0.0 91797.707 435316.878 0.0 91797.414 435316.974 0.0 91797.109 435317.026 0.0 91796.801 435317.031 0.0 91796.494 435316.99 0.0 91796.198 435316.904 0.0 91795.918 435316.774 0.0 91795.66 435316.604 0.0 91795.431 435316.398 0.0 91795.235 435316.159 0.0 91795.076 435315.894 0.0 91794.96 435315.608 0.0 91794.887 435315.308 0.0 91794.86 435315.0 0.0 91794.88 435311.78 0.0 91794.57 435311.781 0.0 91790.84 435311.79 0.0 91790.61 435311.61 0.0 91790.62 435311.17 0.0 91787.789 435311.161 0.0 91787.708 435311.155 0.0 91787.629 435311.142 0.0 91787.551 435311.121 0.0 91787.475 435311.094 0.0 91787.401 435311.061 0.0 91787.331 435311.021 0.0 91787.265 435310.975 0.0 91787.203 435310.923 0.0 91787.146 435310.866 0.0 91787.095 435310.804 0.0 91787.049 435310.738 0.0 91787.009 435310.668 0.0 91786.975 435310.595 0.0 91786.948 435310.519 0.0 91786.928 435310.441 0.0 91786.915 435310.361 0.0 91786.909 435310.281 0.0 91786.91 435310.2 0.0 91786.89 435302.91 0.0 91787.122 435302.27 0.0 91787.88 435301.96 0.0 91790.6 435301.94 0.0 91790.8 435301.32 0.0 91792.29 435301.313 0.0 91809.15 435301.23 0.0 91809.301 435301.841 0.0 91809.37 435302.12 0.0 91810.38 435302.1 0.0 91810.39 435310.8 0.0</gml:posList>
                </gml:LinearRing>
              </gml:exterior>
            </gml:Polygon>
          </bag_LVC:pandGeometrie>
          <bag_LVC:bouwjaar>1942</bag_LVC:bouwjaar>
          <bag_LVC:pandstatus>Pand in gebruik</bag_LVC:pandstatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2015100900000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20151009</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.15/54428</bagtype:documentnummer>
          </bag_LVC:bron>
        </bag_LVC:Pand>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"), new XmlReaderSettings { IgnoreWhitespace = true });

            var startInstant = Instant.FromUtc(2015, 10, 09, 00, 00);
            var expectedPand = new BagPand(
                id: "0599100000661089",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                constructionYear: 1942);

            // act
            var panden = BagParser.ParsePanden(xmlReader).ToList();

            // assert
            panden.Should().ContainSingle();
            panden.Single().Should().BeEquivalentTo(expectedPand);
        }

        [Fact]
        public void ParsePanden_EinddatumBug()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Pand>
          <bag_LVC:identificatie>0599100000661089</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:pandGeometrie>
            <gml:Polygon srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:exterior>
                <gml:LinearRing>
                  <gml:posList srsDimension=""3"" count=""71"">91810.39 435310.8 0.0 91811.73 435310.76 0.0 91811.741 435311.15 0.0 91811.75 435311.47 0.0 91811.691 435312.149 0.0 91811.522 435312.809 0.0 91811.248 435313.433 0.0 91810.876 435314.003 0.0 91810.416 435314.506 0.0 91809.88 435314.927 0.0 91809.283 435315.256 0.0 91808.641 435315.482 0.0 91807.97 435315.602 0.0 91807.61 435315.62 0.0 91807.63 435314.27 0.0 91798.96 435314.3 0.0 91798.98 435314.97 0.0 91798.794 435315.83 0.0 91798.645 435316.101 0.0 91798.457 435316.346 0.0 91798.235 435316.56 0.0 91797.983 435316.739 0.0 91797.707 435316.878 0.0 91797.414 435316.974 0.0 91797.109 435317.026 0.0 91796.801 435317.031 0.0 91796.494 435316.99 0.0 91796.198 435316.904 0.0 91795.918 435316.774 0.0 91795.66 435316.604 0.0 91795.431 435316.398 0.0 91795.235 435316.159 0.0 91795.076 435315.894 0.0 91794.96 435315.608 0.0 91794.887 435315.308 0.0 91794.86 435315.0 0.0 91794.88 435311.78 0.0 91794.57 435311.781 0.0 91790.84 435311.79 0.0 91790.61 435311.61 0.0 91790.62 435311.17 0.0 91787.789 435311.161 0.0 91787.708 435311.155 0.0 91787.629 435311.142 0.0 91787.551 435311.121 0.0 91787.475 435311.094 0.0 91787.401 435311.061 0.0 91787.331 435311.021 0.0 91787.265 435310.975 0.0 91787.203 435310.923 0.0 91787.146 435310.866 0.0 91787.095 435310.804 0.0 91787.049 435310.738 0.0 91787.009 435310.668 0.0 91786.975 435310.595 0.0 91786.948 435310.519 0.0 91786.928 435310.441 0.0 91786.915 435310.361 0.0 91786.909 435310.281 0.0 91786.91 435310.2 0.0 91786.89 435302.91 0.0 91787.122 435302.27 0.0 91787.88 435301.96 0.0 91790.6 435301.94 0.0 91790.8 435301.32 0.0 91792.29 435301.313 0.0 91809.15 435301.23 0.0 91809.301 435301.841 0.0 91809.37 435302.12 0.0 91810.38 435302.1 0.0 91810.39 435310.8 0.0</gml:posList>
                </gml:LinearRing>
              </gml:exterior>
            </gml:Polygon>
          </bag_LVC:pandGeometrie>
          <bag_LVC:bouwjaar>1942</bag_LVC:bouwjaar>
          <bag_LVC:pandstatus>Pand in gebruik</bag_LVC:pandstatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2015100900000000</bagtype:begindatumTijdvakGeldigheid>
            <bagtype:einddatumTijdvakGeldigheid>2016100900000000</bagtype:einddatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20151009</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.15/54428</bagtype:documentnummer>
          </bag_LVC:bron>
        </bag_LVC:Pand>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2015, 10, 09, 00, 00);
            var endInstant = Instant.FromUtc(2016, 10, 09, 00, 00);
            var expectedPand = new BagPand(
                id: "0599100000661089",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, endInstant)),
                constructionYear: 1942);

            // act
            var panden = BagParser.ParsePanden(xmlReader).ToList();

            // assert
            panden.Should().ContainSingle();
            panden.Single().Should().BeEquivalentTo(expectedPand);
        }

        [Fact]
        public void ParseVerblijfsobject_OnlySingleRelatedPandAndAddress()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Verblijfsobject>
          <bag_LVC:gerelateerdeAdressen>
            <bag_LVC:hoofdadres>
              <bag_LVC:identificatie>0599200000300941</bag_LVC:identificatie>
            </bag_LVC:hoofdadres>
          </bag_LVC:gerelateerdeAdressen>
          <bag_LVC:identificatie>0599010000253867</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:verblijfsobjectGeometrie>
            <gml:Point srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:pos>95778.98 435685.07 0.0</gml:pos>
            </gml:Point>
          </bag_LVC:verblijfsobjectGeometrie>
          <bag_LVC:gebruiksdoelVerblijfsobject>overige gebruiksfunctie</bag_LVC:gebruiksdoelVerblijfsobject>
          <bag_LVC:oppervlakteVerblijfsobject>32</bag_LVC:oppervlakteVerblijfsobject>
          <bag_LVC:verblijfsobjectStatus>Verblijfsobject in gebruik</bag_LVC:verblijfsobjectStatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>1993113000000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>19750101</bagtype:documentdatum>
            <bagtype:documentnummer>B2--0130--92</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:gerelateerdPand>
            <bag_LVC:identificatie>0599100000634909</bag_LVC:identificatie>
          </bag_LVC:gerelateerdPand>
        </bag_LVC:Verblijfsobject>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(1993, 11, 30, 00, 00);
            var expectedVerblijfsobject = new BagVerblijfsobject(
                id: "0599010000253867",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                relatedMainAddress: "0599200000300941",
                relatedPanden: "0599100000634909");

            // act
            var verblijfsobjecten = BagParser.ParseVerblijfsobjecten(xmlReader);

            // assert
            verblijfsobjecten.Should().ContainSingle();
            verblijfsobjecten.Single().Should().BeEquivalentTo(expectedVerblijfsobject);
        }

        [Fact]
        public void ParseVerblijfsobject_MultipleOfRelatedPandenAndAddresses()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Verblijfsobject>
          <bag_LVC:gerelateerdeAdressen>
            <bag_LVC:hoofdadres>
              <bag_LVC:identificatie>0599200000300941</bag_LVC:identificatie>
            </bag_LVC:hoofdadres>
            <bag_LVC:nevenadres>
              <bag_LVC:identificatie>0014200022197685</bag_LVC:identificatie>
            </bag_LVC:nevenadres>
            <bag_LVC:nevenadres>
              <bag_LVC:identificatie>0014200040356445</bag_LVC:identificatie>
            </bag_LVC:nevenadres>
          </bag_LVC:gerelateerdeAdressen>
          <bag_LVC:identificatie>0599010000253867</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:verblijfsobjectGeometrie>
            <gml:Point srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:pos>95778.98 435685.07 0.0</gml:pos>
            </gml:Point>
          </bag_LVC:verblijfsobjectGeometrie>
          <bag_LVC:gebruiksdoelVerblijfsobject>overige gebruiksfunctie</bag_LVC:gebruiksdoelVerblijfsobject>
          <bag_LVC:oppervlakteVerblijfsobject>32</bag_LVC:oppervlakteVerblijfsobject>
          <bag_LVC:verblijfsobjectStatus>Verblijfsobject in gebruik</bag_LVC:verblijfsobjectStatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>1993113000000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>19750101</bagtype:documentdatum>
            <bagtype:documentnummer>B2--0130--92</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:gerelateerdPand>
            <bag_LVC:identificatie>0599100000634909</bag_LVC:identificatie>
          </bag_LVC:gerelateerdPand>
          <bag_LVC:gerelateerdPand>
            <bag_LVC:identificatie>0599100000634908</bag_LVC:identificatie>
          </bag_LVC:gerelateerdPand>
        </bag_LVC:Verblijfsobject>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(1993, 11, 30, 00, 00);
            var expectedVerblijfsobject = new BagVerblijfsobject(
                id: "0599010000253867",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                relatedMainAddress: "0599200000300941",
                relatedAdditionalAddresses: ImmutableList.Create(
                    "0014200022197685",
                    "0014200040356445"),
                relatedPanden: ImmutableList.Create(
                    "0599100000634909",
                    "0599100000634908"));

            // act
            var verblijfsobjecten = BagParser.ParseVerblijfsobjecten(xmlReader);

            // assert
            verblijfsobjecten.Should().ContainSingle();
            verblijfsobjecten.Single().Should().BeEquivalentTo(expectedVerblijfsobject);
        }

        [Fact]
        public void ParseNummberaanduidingen_AllOptionalValuesAbsent()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Nummeraanduiding>
          <bag_LVC:identificatie>0000200000057534</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:huisnummer>32</bag_LVC:huisnummer>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2018032600000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:typeAdresseerbaarObject>Verblijfsobject</bag_LVC:typeAdresseerbaarObject>
          <bag_LVC:bron>
            <bagtype:documentdatum>20180326</bagtype:documentdatum>
            <bagtype:documentnummer>BV05.00043-HLG</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:nummeraanduidingStatus>Naamgeving uitgegeven</bag_LVC:nummeraanduidingStatus>
          <bag_LVC:gerelateerdeOpenbareRuimte>
            <bag_LVC:identificatie>1883300000001522</bag_LVC:identificatie>
          </bag_LVC:gerelateerdeOpenbareRuimte>
        </bag_LVC:Nummeraanduiding>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2018, 03, 26, 00, 00);
            var expectedNummeraanduiding = new BagNummeraanduiding(
                id: "0000200000057534",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                houseNumber: 32,
                houseLetter: null,
                houseNumberAddition: null,
                postalCode: null,
                relatedOpenbareRuimte: "1883300000001522");

            // act
            var nummeraanduiding = BagParser.ParseNummeraanduidingen(xmlReader);

            // assert
            nummeraanduiding.Should().ContainSingle();
            nummeraanduiding.Single().Should().BeEquivalentTo(expectedNummeraanduiding);
        }

        [Fact]
        public void ParseNummberaanduidingen_WithOptionalValuesPresent()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Nummeraanduiding>
          <bag_LVC:identificatie>0000200000057534</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:huisnummer>32</bag_LVC:huisnummer>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:huisletter>A</bag_LVC:huisletter>
          <bag_LVC:huisnummertoevoeging>123</bag_LVC:huisnummertoevoeging>
          <bag_LVC:postcode>6131BE</bag_LVC:postcode>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2018032600000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:typeAdresseerbaarObject>Verblijfsobject</bag_LVC:typeAdresseerbaarObject>
          <bag_LVC:bron>
            <bagtype:documentdatum>20180326</bagtype:documentdatum>
            <bagtype:documentnummer>BV05.00043-HLG</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:nummeraanduidingStatus>Naamgeving uitgegeven</bag_LVC:nummeraanduidingStatus>
          <bag_LVC:gerelateerdeOpenbareRuimte>
            <bag_LVC:identificatie>1883300000001522</bag_LVC:identificatie>
          </bag_LVC:gerelateerdeOpenbareRuimte>
        </bag_LVC:Nummeraanduiding>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2018, 03, 26, 00, 00);
            var expectedNummeraanduiding = new BagNummeraanduiding(
                id: "0000200000057534",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                houseNumber: 32,
                houseLetter: 'A',
                houseNumberAddition: "123",
                postalCode: "6131BE",
                relatedOpenbareRuimte: "1883300000001522");

            // act
            var nummeraanduiding = BagParser.ParseNummeraanduidingen(xmlReader);

            // assert
            nummeraanduiding.Should().ContainSingle();
            nummeraanduiding.Single().Should().BeEquivalentTo(expectedNummeraanduiding);
        }

        [Fact]
        public void ParseOpenbareRuimte()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:OpenbareRuimte>
          <bag_LVC:identificatie>0003300000116985</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:openbareRuimteNaam>Abel Eppensstraat</bag_LVC:openbareRuimteNaam>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>1956032800000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:openbareRuimteType>Weg</bag_LVC:openbareRuimteType>
          <bag_LVC:bron>
            <bagtype:documentdatum>19560328</bagtype:documentdatum>
            <bagtype:documentnummer>OR RB 28-03-1956</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:openbareruimteStatus>Naamgeving uitgegeven</bag_LVC:openbareruimteStatus>
          <bag_LVC:gerelateerdeWoonplaats>
            <bag_LVC:identificatie>3386</bag_LVC:identificatie>
          </bag_LVC:gerelateerdeWoonplaats>
        </bag_LVC:OpenbareRuimte>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(1956, 03, 28, 00, 00);
            var expectedOpenbareRuimte = new BagOpenbareRuimte(
                id: "0003300000116985",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                name: "Abel Eppensstraat",
                relatedWoonplaats: "3386");

            // act
            var openbareRuimte = BagParser.ParseOpenbareRuimten(xmlReader);

            // assert
            openbareRuimte.Should().ContainSingle();
            openbareRuimte.Single().Should().BeEquivalentTo(expectedOpenbareRuimte);
        }

        [Fact]
        public void ParseWoonplaats()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Woonplaats>
          <bag_LVC:identificatie>3086</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:woonplaatsNaam>Rotterdam</bag_LVC:woonplaatsNaam>
          <bag_LVC:woonplaatsGeometrie>
            <gml:Polygon srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:exterior>
                <gml:LinearRing>
                  <gml:posList srsDimension=""2"" count=""3890"">85730.998 442144.763 85751.465 442083.765 85754.122 442075.657 85764.738 442043.255 85768.301 442032.381 85768.638 442031.306 85769.786 442027.647 85771.075 442023.536 85778.458 442000.0 85793.418 441952.309 85822.572 441859.28 85844.451 441789.463 85844.51 441789.276 85852.204 441764.723 85866.273 441718.381 85886.279 441652.482 85886.453 441651.91 85886.494 441651.774 85901.289 441607.866 85901.982 441605.809 85903.105 441602.478 85911.863 441574.82 85929.809 441518.144 85929.926 441517.776 85931.063 441514.186 85932.178 441510.664 85935.538 441500.054 85935.555 441500.0 85942.07 441479.427 85964.083 441413.844 85968.411 441400.949 85996.828 441308.937 86000.0 441298.666 86015.46 441248.608 86023.024 441224.706 86038.006 441177.364 86038.596 441175.5 86040.614 441169.125 86041.5 441166.325 86042.621 441162.781 86068.498 441081.011 86068.88 441079.804 86068.944 441079.603 86069.864 441076.697 86084.031 441031.928 86090.39 441011.835 86094.126 441000.0 86098.547 440985.997 86112.566 440941.592 86145.442 440837.452 86146.311 440834.701 86146.489 440834.138 86186.788 440706.491 86191.645 440691.104 86215.065 440618.691 86235.124 440556.669 86242.535 440533.754 86248.524 440515.235 86248.548 440515.162 86253.452 440500.0 86272.165 440442.137 86273.148 440439.098 86273.787 440437.121 86274.341 440435.407 86275.02 440433.309 86276.628 440428.337 86281.025 440414.742 86292.377 440379.64 86292.444 440379.434 86301.689 440350.848 86311.234 440318.543 86312.146 440315.457 86312.509 440314.229 86314.966 440305.914 86316.905 440299.352 86320.543 440287.039 86323.444 440277.223 86324.023 440275.263 86331.375 440250.381 86331.389 440250.332 86331.517 440249.9 86331.574 440249.708 86331.612 440249.58 86338.802 440225.246 86338.816 440225.198 86363.477 440146.867 86365.07 440147.426 86390.79 440156.449 86392.706 440157.121 86404.184 440152.867 86410.836 440145.114 86415.75 440139.746 86421.321 440142.466 86422.363 440142.975 86426.086 440144.792 86428.012 440145.732 86428.117 440145.784 86433.283 440148.306 86434.218 440148.797 86449.554 440156.858 86460.208 440162.458 86465.855 440165.426 86482.356 440174.1 86605.236 440242.881 86713.138 440303.277 86713.284 440303.359 86719.547 440306.865 86739.996 440317.498 86740.281 440317.393 86769.379 440306.656 86769.67 440306.548 86774.112 440304.904 86794.074 440297.544 86795.614 440296.975 86797.184 440296.396 86794.55 440288.65 86793.172 440284.945 86778.103 440244.414 86739.42 440140.37 86689.24 440040.35 86679.04 440030.5 86675.91 440024.11 86673.51 440001.27 86675.831 440000.0 86702.73 439985.28 86715.52 439982.22 86725.92 439971.62 86776.59 439849.56 86778.531 439847.708 86964.562 439670.178 86966.706 439668.132 86972.65 439662.46 87000.0 439630.029 87013.72 439613.76 87031.85 439590.07 87066.76 439550.05 87074.86 439539.66 87084.12 439530.11 87093.71 439517.37 87102.81 439507.07 87109.306 439500.0 87123.679 439484.356 87142.95 439463.38 87149.03 439458.55 87170.22 439435.99 87203.89 439428.88 87215.47 439427.622 87223.13 439426.79 87235.254 439425.589 87236.937 439425.422 87236.967 439425.419 87238.47 439425.27 87248.394 439414.465 87255.905 439406.288 87261.35 439400.36 87262.262 439398.926 87262.369 439398.757 87263.78 439396.54 87263.711 439395.816 87262.089 439378.771 87258.89 439345.16 87261.12 439341.45 87263.66 439338.29 87382.96 439211.96 87514.67 439277.89 87531.94 439270.95 87556.0 439283.846 87562.692 439287.434 87633.722 439325.507 87701.17 439361.66 87709.575 439350.201 87740.667 439307.812 87755.17 439288.04 87758.925 439289.155 87794.677 439299.774 87798.773 439300.991 87819.296 439307.086 87824.567 439308.651 87857.76 439318.51 87864.091 439321.767 87867.667 439323.607 87879.964 439329.934 87886.409 439333.25 87886.535 439333.315 87904.128 439342.367 87904.181 439342.394 87919.665 439350.361 87927.357 439354.318 87938.692 439360.15 88000.0 439391.693 88024.27 439404.18 88066.49 439440.18 88092.51 439465.06 88109.015 439500.0 88118.67 439520.44 88122.942 439525.079 88183.91 439591.29 88184.506 439590.088 88186.164 439586.745 88199.645 439559.565 88206.123 439546.504 88224.909 439508.628 88229.188 439500.0 88231.97 439494.392 88253.1 439451.79 88255.606 439451.809 88256.458 439451.816 88257.0 439451.82 88261.17 439446.5 88299.4 439389.77 88307.866 439376.529 88309.235 439374.389 88309.33 439374.24 88320.59 439355.91 88332.861 439335.965 88333.219 439335.384 88327.796 439331.279 88320.982 439326.183 88315.242 439321.882 88313.638 439320.681 88307.992 439316.456 88287.155 439311.646 88284.667 439311.072 88284.184 439310.96 88266.664 439306.905 88255.382 439301.883 88252.645 439300.664 88246.8 439298.29 88246.538 439298.77 88236.265 439291.942 88236.04 439292.241 88231.112 439289.034 88231.187 439288.758 88227.024 439286.25 88226.861 439286.152 88206.34 439272.6 88185.915 439259.139 88173.334 439250.0 88165.081 439244.005 88151.199 439233.707 88133.341 439219.126 88133.266 439219.026 88129.865 439216.296 88128.688 439215.451 88122.886 439211.284 88108.98 439201.762 88075.463 439178.334 88060.457 439167.635 88044.849 439156.711 88040.046 439153.503 88035.494 439150.846 88031.167 439148.49 88027.389 439146.535 88022.386 439144.153 88017.258 439141.922 88012.48 439140.266 88007.928 439139.21 88003.274 439137.981 88000.0 439137.434 87993.192 439136.296 87988.388 439135.892 87982.033 439135.411 87977.58 439135.257 87968.398 439135.125 87945.304 439135.157 87944.445 439135.15 87931.522 439135.039 87928.022 439135.009 87908.674 439134.631 87905.784 439134.574 87905.031 439134.559 87896.49 439134.468 87887.321 439133.965 87874.928 439133.073 87862.9 439131.966 87847.281 439129.776 87834.72 439126.759 87824.58 439123.287 87812.731 439118.916 87801.011 439113.274 87792.38 439108.583 87786.642 439104.869 87776.946 439097.358 87769.42 439090.38 87768.762 439090.975 87762.623 439084.319 87763.178 439083.603 87753.079 439071.621 87746.846 439062.766 87740.149 439052.513 87738.809 439050.051 87733.414 439039.538 87730.545 439032.476 87726.868 439019.875 87722.386 439000.0 87721.675 438996.847 87720.018 438984.925 87718.81 438977.49 87715.51 438976.42 87715.12 438971.6 87718.49 438969.76 87715.682 438933.153 87713.839 438908.918 87711.587 438889.476 87709.406 438875.005 87703.775 438848.951 87694.963 438809.487 87691.956 438796.27 87688.831 438780.691 87688.69 438779.987 87688.282 438777.954 87684.533 438758.564 87681.515 438741.743 87677.671 438720.321 87673.371 438692.765 87671.321 438676.497 87669.285 438657.423 87668.151 438644.5 87667.06 438628.437 87666.29 438615.21 87665.478 438602.338 87665.752 438586.347 87666.207 438565.07 87666.622 438558.491 87667.721 438549.691 87668.746 438542.4 87670.654 438532.804 87672.635 438524.338 87674.407 438518.688 87677.644 438508.64 87680.709 438500.0 87681.062 438499.004 87688.501 438481.306 87692.563 438472.436 87694.211 438468.744 87699.539 438458.328 87703.841 438451.516 87709.753 438442.336 87721.113 438426.29 87727.35 438417.278 87729.92 438413.311 87738.785 438397.231 87743.611 438388.39 87749.185 438378.442 87750.568 438375.973 87761.325 438355.053 87766.62 438344.58 87769.276 438338.322 87773.36 438328.2 87779.01 438305.72 87780.233 438300.854 87785.66 438303.28 87809.66 438314.0 87813.08 438315.54 87823.255 438320.198 87932.65 438075.326 87937.897 438063.603 87966.365 438000.0 88000.0 437924.852 88028.1 437862.071 88036.092 437844.214 88079.906 437746.324 88134.667 437623.976 88139.759 437612.599 88145.719 437612.937 88154.715 437613.334 88163.035 437613.873 88172.83 437614.577 88183.617 437615.656 88195.712 437616.779 88202.648 437617.634 88205.265 437617.957 88205.271 437617.958 88214.849 437619.124 88222.447 437620.103 88230.225 437621.324 88231.155 437621.475 88232.635 437621.715 88235.653 437622.204 88236.738 437622.38 88243.805 437623.876 88249.72 437625.51 88249.721 437625.02 88249.726 437622.759 88249.765 437605.259 88249.765 437605.25 88249.767 437604.572 88249.84 437572.55 88261.36 437561.07 88261.383 437549.102 88261.387 437546.744 88261.398 437541.098 88261.477 437500.0 88261.7 437384.806 88261.7 437384.721 88261.701 437383.98 88261.706 437381.758 88261.802 437331.544 88261.81 437327.602 88261.811 437327.009 88261.883 437289.67 88261.91 437275.654 88259.039 437261.51 88256.703 437250.0 88252.715 437230.352 88243.468 437184.791 88230.166 437119.253 88214.414 437041.648 88207.843 437009.272 88205.961 437000.0 88171.053 436828.011 88161.085 436829.842 88160.637 436827.637 88144.86 436750.0 88131.379 436683.659 88128.293 436668.472 88121.402 436669.248 88116.543 436669.795 88026.149 436679.981 88014.879 436681.251 88012.027 436653.307 88011.824 436651.319 88011.809 436651.182 88007.409 436610.977 88006.922 436606.527 88000.0 436606.735 87959.162 436607.963 87958.081 436607.995 87957.613 436607.984 87957.187 436607.961 87956.722 436607.917 87956.303 436607.848 87955.912 436607.775 87955.429 436607.662 87955.031 436607.548 87954.577 436607.401 87954.054 436607.194 87953.488 436606.936 87952.926 436606.626 87952.416 436606.301 87951.917 436605.955 87951.396 436605.523 87950.874 436605.035 87950.392 436604.512 87949.993 436604.023 87949.633 436603.51 87949.198 436602.793 87941.542 436588.046 87940.84 436586.674 87940.499 436585.934 87939.764 436584.224 87939.403 436583.298 87938.85 436581.654 87938.565 436580.721 87938.308 436579.83 87938.077 436578.921 87937.793 436577.578 87933.932 436552.176 87933.498 436550.798 87929.738 436538.851 87929.003 436539.032 87928.156 436539.161 87927.277 436539.235 87926.348 436539.197 87925.417 436539.097 87924.422 436538.896 87923.517 436538.608 87922.591 436538.219 87921.829 436537.813 87921.008 436537.267 87920.438 436536.804 87920.43 436536.798 87920.164 436536.582 87919.521 436535.943 87918.525 436534.695 87917.815 436533.72 87917.149 436532.737 87916.431 436531.787 87915.703 436530.895 87914.947 436530.023 87914.132 436529.128 87913.221 436528.167 87912.071 436527.093 87910.756 436525.972 87909.568 436525.018 87908.226 436524.03 87906.928 436523.193 87905.692 436522.444 87904.341 436521.711 87903.229 436521.167 87890.673 436513.899 87866.66 436500.0 87705.533 436406.739 87699.712 436403.37 87724.696 436250.0 87729.441 436220.869 87759.998 436158.071 87765.173 436160.595 87768.247 436162.094 87771.202 436163.535 87853.304 436203.581 87946.841 436010.11 87946.916 436009.955 87951.742 436000.0 87993.881 435913.069 87993.427 435909.176 87993.126 435906.599 87991.73 435898.23 87991.435 435896.749 87991.126 435895.271 87990.802 435893.796 87990.709 435893.391 87990.69 435893.302 87990.662 435893.188 87990.463 435892.324 87990.111 435890.856 87989.744 435889.391 87989.517 435888.523 87989.489 435888.41 87989.463 435888.316 87989.362 435887.929 87988.967 435886.472 87988.557 435885.019 87988.133 435883.569 87988.13 435883.56 87987.694 435882.124 87987.242 435880.683 87986.776 435879.247 87986.639 435878.84 87986.615 435878.763 87986.582 435878.668 87986.295 435877.815 87985.801 435876.388 87985.292 435874.966 87984.979 435874.116 87984.944 435874.018 87984.914 435873.941 87984.77 435873.549 87984.234 435872.137 87983.684 435870.731 87983.12 435869.33 88000.0 435834.017 88004.527 435824.546 88004.64 435824.31 88034.379 435780.226 88109.943 435668.255 88121.135 435651.67 88124.485 435644.384 88143.474 435615.757 88049.106 435500.0 88000.0 435439.764 87909.847 435329.177 88000.0 435146.132 88001.781 435142.516 88000.0 435141.975 87999.62 435141.86 87803.28 435073.62 87725.33 435046.446 87608.063 435005.566 87603.951 435004.136 87592.062 435000.0 87201.434 434864.105 87000.0 434789.724 86986.72 434784.82 86958.93 434776.85 86923.95 434767.23 86910.867 434763.993 86887.62 434758.24 86846.95 434749.07 86805.637 434740.643 87931.605 432478.312 87989.989 432253.755 87902.637 431912.31 87913.7 431912.986 87946.552 431914.322 87957.776 431915.02 88000.0 431925.347 88001.824 431925.793 88022.084 431930.747 88023.438 431929.803 88047.455 431913.065 88090.063 431970.041 88094.546 431976.035 88094.628 431976.145 88099.639 431982.846 88101.326 431985.101 88105.127 431990.184 88112.042 431990.544 88121.905 431991.058 88125.337 431991.237 88147.981 431991.666 88182.447 431991.165 88199.188 431989.394 88228.718 431986.84 88265.134 431981.201 88293.011 431973.833 88332.195 431964.115 88362.303 431956.329 88412.328 431942.742 88474.055 431924.926 88500.0 431917.43 88546.119 431904.106 88623.607 431881.783 88704.331 431857.208 88759.804 431837.701 88797.099 431826.218 88840.223 431813.179 88870.575 431803.664 88890.338 431797.713 88917.075 431786.188 88935.891 431774.709 88940.947 431770.589 88955.343 431758.86 88965.698 431750.0 88965.995 431749.746 88975.952 431741.227 89000.0 431720.639 89016.31 431706.676 89083.367 431648.307 89083.631 431648.078 89092.594 431640.275 89112.573 431647.665 89148.375 431662.775 89150.126 431663.514 89197.079 431683.331 89235.344 431668.58 89291.742 431646.667 89374.188 431615.037 89400.18 431602.87 89525.42 431541.61 89590.84 431502.22 89587.159 431500.0 89561.581 431484.575 89560.23 431483.76 89561.08 431483.192 89561.199 431483.112 89596.652 431459.381 89618.302 431444.89 89635.05 431433.68 89639.81 431433.3 89696.56 431386.82 89767.602 431309.613 89767.78 431309.42 89768.663 431308.462 89771.53 431305.353 89771.534 431305.348 89777.845 431298.502 89778.211 431298.106 89825.782 431246.641 89827.82 431244.436 89829.391 431242.74 89830.79 431241.23 89879.17 431265.58 89882.17 431267.06 89896.36 431272.93 89897.643 431273.397 89906.74 431276.709 89913.91 431279.32 89951.59 431284.32 90000.0 431284.758 90002.317 431284.779 90059.81 431285.3 90102.6 431279.19 90107.213 431277.988 90125.754 431273.156 90206.71 431252.06 90472.728 431156.056 90594.86 431111.98 90682.798 431081.134 90687.748 431079.398 90687.903 431079.343 90690.65 431078.38 90882.807 431000.0 91000.19 430952.12 91174.13 430881.87 91202.898 430871.021 91211.801 430867.664 91284.057 430840.416 91345.57 430822.71 91386.39 430811.83 91432.69 430800.06 91473.73 430790.85 91492.61 430786.91 91500.99 430790.15 91504.34 430791.64 91583.2 430776.6 91639.46 430766.37 91678.58 430760.74 91698.46 430758.24 91743.799 430754.916 91890.23 430744.18 91949.08 430742.06 92000.0 430745.199 92056.63 430748.69 92120.76 430764.61 92141.92 430769.68 92322.3 430774.06 92325.438 430774.553 92509.07 430803.42 92538.22 430808.15 92538.145 430811.033 92537.316 430842.901 92536.529 430873.109 92536.379 430878.89 92535.823 430900.243 92535.79 430901.53 92535.64 430903.84 92534.72 430913.13 92534.57 430914.556 92534.558 430914.673 92534.361 430916.533 92534.35 430916.64 92534.065 430922.673 92533.852 430927.175 92533.64 430931.676 92533.378 430937.308 92533.14 430942.855 92532.98 430946.6 92532.983 430947.348 92533.003 430951.84 92533.02 430955.694 92533.024 430956.59 92533.024 430956.61 92533.027 430957.308 92533.03 430958.07 92533.002 430961.582 92533.0 430961.858 92532.997 430962.236 92532.992 430962.798 92532.957 430967.268 92532.921 430971.758 92532.873 430977.81 92532.83 430998.24 92532.715 431000.0 92532.629 431001.31 92532.59 431001.912 92531.322 431021.334 92530.941 431027.176 92530.522 431033.602 92530.106 431039.977 92530.057 431040.72 92530.02 431041.29 92529.868 431047.586 92529.702 431054.451 92529.691 431054.906 92529.524 431061.838 92529.37 431068.22 92529.339 431069.743 92529.21 431076.183 92529.07 431083.17 92529.092 431090.848 92529.11 431097.12 92529.079 431100.106 92529.02 431105.85 92528.83 431110.0 92528.807 431110.731 92528.74 431112.87 92527.29 431117.32 92523.752 431124.876 92523.68 431125.03 92523.481 431125.352 92519.855 431131.21 92518.19 431133.9 92516.63 431135.44 92514.38 431135.91 92508.76 431136.41 92524.895 431136.717 92529.29 431136.8 92538.76 431137.21 92544.904 431136.753 92546.215 431136.656 92553.94 431136.14 92564.73 431134.81 92565.6 431137.77 92567.526 431139.078 92567.75 431139.23 92570.77 431140.01 92578.26 431140.41 92579.736 431140.884 92579.91 431140.94 92581.326 431141.446 92587.35 431143.6 92590.18 431145.29 92591.88 431146.0 92597.54 431148.29 92602.69 431150.08 92614.34 431152.38 92616.067 431152.474 92617.11 431152.53 92631.956 431152.956 92632.11 431152.96 92636.05 431154.01 92662.94 431164.81 92672.24 431168.78 92694.56 431178.867 92695.01 431179.07 92701.441 431181.928 92707.061 431184.037 92708.203 431184.45 92713.99 431186.54 92725.38 431190.35 92759.5 431203.5 92768.4 431207.47 92771.706 431209.112 92778.59 431212.53 92782.86 431214.86 92786.96 431217.21 92790.56 431219.18 92795.05 431221.44 92796.76 431222.25 92803.471 431225.021 92805.13 431225.706 92808.41 431227.06 92819.87 431232.21 92828.07 431235.84 92830.53 431237.2 92839.54 431241.96 92840.844 431242.665 92841.26 431242.89 92842.17 431242.32 92845.03 431243.41 92846.13 431245.1 92881.876 431265.134 92893.66 431271.36 92905.255 431276.44 92905.46 431276.53 92905.53 431276.431 92906.56 431274.98 92909.56 431276.74 92908.51 431278.42 92915.35 431283.57 92917.63 431285.07 92932.54 431294.72 92939.207 431298.517 92948.434 431303.771 92957.51 431308.94 92959.39 431310.06 92961.13 431310.75 92963.565 431312.111 92971.473 431317.301 92976.44 431320.56 92982.641 431325.149 92984.86 431326.79 92986.94 431326.51 92989.69 431328.21 92989.44 431328.66 92989.77 431329.71 92994.98 431333.15 92996.954 431334.124 93000.0 431335.627 93011.38 431341.24 93021.23 431343.06 93023.93 431343.2 93032.73 431343.82 93037.058 431344.221 93037.69 431344.28 93047.98 431346.62 93054.19 431348.09 93069.29 431350.98 93071.5 431351.44 93080.27 431352.28 93088.254 431353.397 93102.28 431355.08 93107.48 431356.1 93115.42 431358.35 93120.27 431359.55 93133.05 431366.96 93145.48 431372.63 93147.03 431373.11 93146.74 431374.23 93147.77 431376.47 93149.653 431377.778 93153.17 431380.22 93176.76 431393.15 93193.17 431399.96 93194.09 431398.17 93197.18 431399.26 93196.68 431400.98 93220.55 431412.01 93233.966 431417.662 93245.12 431422.36 93258.83 431429.51 93260.54 431428.86 93264.06 431430.6 93287.092 431441.294 93300.899 431448.769 93321.896 431461.449 93330.846 431466.384 93336.126 431469.295 93345.049 431472.388 93348.638 431474.228 93355.175 431476.688 93366.353 431478.844 93373.709 431480.73 93381.447 431482.714 93388.357 431484.688 93388.656 431484.773 93399.121 431488.542 93406.749 431491.633 93409.382 431492.547 93412.136 431493.504 93417.308 431496.372 93422.209 431499.09 93423.735 431500.0 93428.784 431503.011 93438.764 431508.963 93462.92 431523.368 93474.288 431527.86 93480.411 431531.541 93481.371 431532.119 93483.549 431533.429 93484.687 431534.113 93485.498 431534.6 93486.645 431535.29 93487.169 431535.284 93490.66 431535.253 93500.0 431535.17 93503.866 431535.135 93511.795 431535.065 93523.975 431534.958 93528.882 431534.915 93536.633 431534.847 93540.741 431534.811 93545.863 431534.765 93586.168 431534.349 93586.167 431538.303 93586.167 431539.445 93631.611 431539.495 93631.239 431538.139 93629.987 431533.574 93636.676 431533.567 93780.469 431533.427 93917.697 431532.628 93917.72 431532.628 93941.566 431532.489 93962.178 431532.369 94000.0 431532.149 94013.599 431532.07 94036.337 431531.857 94038.757 431531.835 94043.011 431531.795 94155.516 431530.745 94219.416 431530.257 94303.827 431529.612 94304.759 431529.605 94381.884 431529.016 94391.219 431528.93 94470.82 431528.201 94500.0 431527.934 94509.89 431527.843 94519.08 431527.759 94552.327 431527.454 94598.846 431527.028 94607.892 431526.945 94701.322 431526.31 94756.944 431525.761 94765.857 431525.673 94774.099 431525.592 94884.003 431525.168 94946.706 431525.182 94953.205 431525.183 94956.363 431525.184 94964.141 431525.185 94968.654 431525.186 94982.997 431525.146 94984.005 431525.143 95000.0 431525.098 95057.324 431524.936 95060.389 431524.92 95072.581 431524.855 95107.628 431524.67 95240.84 431523.408 95252.982 431523.293 95356.41 431522.129 95400.377 431521.343 95400.38 431521.343 95455.812 431523.711 95495.227 431529.405 95498.799 431529.921 95500.0 431530.116 95513.447 431532.305 95566.812 431545.244 95588.874 431551.68 95595.331 431553.564 95596.57 431553.933 95617.192 431560.071 95618.173 431560.514 95671.415 431584.559 95682.863 431589.844 95688.017 431592.524 95689.963 431593.536 95693.164 431592.618 95694.525 431592.228 95695.262 431593.494 95700.483 431588.734 95705.347 431585.409 95715.823 431577.532 95735.861 431563.735 95738.327 431562.346 95754.702 431553.359 95757.208 431554.726 95763.239 431554.396 95771.231 431550.718 95780.271 431547.293 95830.668 431528.835 95838.557 431528.431 95839.371 431526.254 95841.812 431525.981 95843.158 431527.684 95856.491 431525.637 95862.774 431524.988 95873.284 431525.274 95880.389 431525.467 95887.748 431522.365 95896.936 431518.287 95902.016 431516.279 95907.416 431513.587 95909.698 431512.53 95915.352 431509.912 95917.739 431508.807 95923.357 431506.065 95928.967 431503.692 95940.366 431500.971 95944.77 431500.0 95960.126 431496.614 95960.547 431494.66 95962.932 431494.107 95963.725 431495.758 95991.505 431492.838 96000.0 431493.597 96002.553 431493.825 96005.228 431491.645 96019.973 431490.731 96020.733 431488.777 96023.332 431488.614 96024.266 431490.321 96039.24 431488.712 96055.493 431486.829 96062.886 431485.973 96064.259 431483.019 96067.5 431482.827 96068.356 431484.405 96069.816 431484.411 96075.167 431484.057 96084.992 431483.408 96102.484 431482.535 96122.678 431486.7 96147.285 431491.775 96155.662 431493.753 96167.887 431496.853 96168.374 431497.003 96178.1 431500.0 96213.306 431510.849 96244.884 431520.924 96253.795 431523.768 96264.602 431527.646 96269.057 431529.394 96294.924 431539.498 96295.71 431539.805 96311.22 431545.866 96311.43 431546.045 96312.165 431546.326 96314.924 431547.382 96324.41 431540.961 96329.917 431543.029 96331.708 431548.514 96332.253 431550.182 96332.278 431550.26 96332.614 431551.288 96345.376 431557.394 96363.232 431565.938 96364.586 431566.586 96368.133 431568.283 96375.935 431571.074 96382.838 431574.106 96387.7 431575.604 96392.619 431577.12 96399.8 431579.82 96400.535 431580.096 96407.222 431582.684 96408.285 431582.911 96417.333 431585.002 96418.294 431585.037 96430.163 431585.576 96433.955 431585.748 96447.53 431587.937 96448.905 431588.159 96462.825 431591.173 96463.958 431591.539 96466.356 431592.312 96484.581 431598.305 96490.351 431599.277 96490.521 431599.306 96490.875 431599.366 96494.257 431599.935 96496.577 431600.023 96497.872 431600.073 96508.054 431600.463 96513.448 431600.516 96514.508 431600.527 96521.189 431600.326 96522.54 431600.4 96529.44 431600.284 96530.844 431600.26 96534.132 431600.407 96538.85 431600.627 96543.97 431601.122 96552.414 431602.698 96557.699 431603.685 96563.513 431604.823 96569.417 431606.156 96572.159 431606.775 96585.902 431609.369 96593.581 431610.835 96594.121 431610.938 96599.181 431612.432 96612.412 431616.337 96615.262 431617.178 96629.99 431621.67 96635.246 431624.438 96647.268 431630.113 96651.468 431632.095 96652.525 431632.594 96652.602 431632.634 96653.173 431632.93 96654.182 431633.452 96657.685 431634.835 96673.515 431641.082 96675.594 431641.903 96686.178 431645.03 96686.282 431645.061 96699.973 431650.979 96711.949 431656.156 96726.063 431662.578 96758.705 431677.374 96776.765 431685.282 96783.26 431687.709 96787.852 431689.292 96803.267 431694.605 96815.765 431698.123 96829.201 431699.955 96831.812 431698.982 96835.653 431704.091 96835.796 431704.281 96841.541 431711.925 96841.814 431712.288 96851.82 431725.601 96859.254 431735.49 96888.523 431754.81 96888.901 431755.457 96901.259 431770.009 96906.704 431776.875 96910.765 431782.279 96916.268 431790.134 96919.374 431795.09 96920.032 431796.14 96923.428 431802.273 96926.601 431808.309 96926.694 431808.487 96928.709 431812.923 96933.813 431822.938 96938.068 431832.913 96942.136 431843.438 96944.228 431849.889 96946.076 431855.586 96946.142 431855.81 96948.853 431864.945 96952.701 431864.904 96964.269 431864.782 96967.186 431864.748 96967.351 431864.512 96968.719 431864.73 96985.319 431864.532 96996.287 431865.33 97000.0 431869.512 97006.246 431876.548 97006.943 431877.181 97007.259 431877.468 97024.238 431890.508 97024.96 431891.055 97031.855 431896.279 97034.89 431898.545 97035.046 431898.68 97038.836 431901.95 97041.055 431903.521 97044.199 431905.747 97044.734 431906.126 97046.771 431907.568 97055.026 431913.412 97078.777 431930.573 97078.979 431930.721 97086.687 431936.374 97088.916 431937.908 97089.487 431938.301 97092.114 431940.11 97097.328 431943.55 97097.583 431943.718 97098.308 431944.196 97107.343 431950.156 97112.112 431952.954 97119.499 431957.615 97124.316 431960.743 97127.86 431963.044 97127.921 431963.084 97128.89 431963.713 97134.053 431967.122 97135.149 431967.846 97141.634 431972.187 97147.953 431976.398 97154.297 431980.501 97160.614 431984.471 97165.864 431987.878 97166.886 431988.401 97169.082 431989.656 97172.665 431991.892 97181.879 431997.789 97185.134 432000.0 97186.005 432000.592 97191.415 432004.83 97195.628 432008.57 97197.649 432010.68 97199.227 432012.542 97199.408 432012.762 97203.61 432017.866 97203.809 432018.108 97206.583 432021.646 97207.966 432023.191 97208.187 432023.438 97211.346 432026.254 97212.2 432026.915 97212.423 432027.076 97216.741 432030.199 97217.094 432030.446 97221.79 432032.363 97222.127 432032.501 97224.37 432033.27 97228.637 432034.379 97233.155 432035.468 97235.891 432036.128 97236.796 432036.458 97236.913 432036.5 97236.984 432036.526 97249.293 432041.134 97257.462 432044.388 97262.651 432046.004 97264.523 432046.514 97273.363 432047.803 97280.094 432048.785 97304.552 432052.741 97295.834 432072.164 97297.33 432073.015 97325.08 432088.79 97332.72 432092.53 97353.654 432100.425 97354.01 432100.57 97357.059 432102.162 97361.46 432104.46 97362.29 432104.927 97366.3 432107.18 97368.414 432108.449 97372.02 432110.608 97375.36 432111.45 97382.18 432113.36 97383.11 432113.545 97384.99 432113.92 97398.01 432115.95 97399.841 432116.482 97402.62 432117.29 97410.46 432120.57 97413.08 432121.54 97416.083 432122.083 97416.268 432122.095 97423.58 432121.81 97431.047 432120.981 97433.45 432120.714 97437.63 432120.25 97439.05 432120.189 97450.61 432119.69 97460.41 432120.17 97465.62 432121.3 97472.83 432122.41 97481.26 432125.09 97487.89 432127.6 97492.13 432130.33 97493.121 432131.134 97493.797 432131.683 97500.0 432136.716 97500.83 432137.39 97502.237 432138.211 97520.436 432148.824 97520.622 432148.933 97521.2 432149.27 97523.62 432151.03 97524.444 432151.44 97530.254 432154.331 97540.572 432159.466 97541.952 432160.153 97542.75 432160.55 97559.43 432166.91 97564.73 432169.204 97575.423 432173.834 97575.53 432173.88 97581.75 432175.59 97587.365 432177.985 97587.829 432178.183 97606.69 432186.23 97629.02 432196.31 97634.05 432198.43 97639.66 432199.76 97664.41 432210.4 97671.44 432213.1 97679.87 432215.78 97680.223 432216.024 97680.29 432216.07 97684.52 432218.91 97696.05 432228.32 97720.96 432243.99 97729.799 432249.542 97730.528 432250.0 97732.98 432251.54 97738.853 432254.676 97740.64 432255.63 97740.706 432255.667 97753.705 432262.889 97764.303 432268.962 97774.788 432274.648 97775.904 432275.294 97780.131 432277.741 97797.649 432288.729 97799.192 432289.159 97802.341 432291.398 97806.49 432295.23 97806.634 432295.353 97809.22 432297.56 97816.93 432305.23 97831.71 432317.98 97854.373 432336.772 97854.48 432336.86 97860.606 432342.097 97874.619 432354.078 97884.42 432362.876 97886.334 432364.423 97890.965 432367.804 97900.312 432373.516 97909.453 432378.512 97909.779 432378.69 97909.98 432378.8 97913.81 432380.83 97920.05 432383.54 97922.18 432384.91 97937.378 432394.574 97938.63 432395.37 97947.445 432400.916 97949.31 432402.09 97949.938 432402.547 97951.178 432403.448 97959.81 432409.72 97963.162 432411.687 97963.56 432411.92 97977.76 432420.24 98000.0 432432.162 98008.28 432436.6 98014.74 432439.81 98017.96 432440.76 98018.387 432440.946 98019.64 432441.49 98027.05 432444.74 98043.64 432454.61 98053.486 432460.184 98063.197 432465.681 98063.46 432465.83 98075.236 432471.265 98075.29 432471.29 98077.59 432472.34 98089.32 432478.98 98102.95 432485.19 98122.34 432495.22 98130.276 432500.0 98130.94 432500.4 98133.963 432503.012 98136.02 432504.79 98137.34 432505.81 98141.21 432507.64 98146.0 432509.47 98150.47 432510.86 98160.44 432513.59 98172.01 432516.14 98184.63 432517.536 98186.29 432517.72 98188.59 432517.85 98191.29 432517.79 98207.3 432516.89 98221.8 432516.38 98227.36 432516.977 98235.5 432517.85 98250.82 432517.44 98273.44 432516.03 98286.32 432514.69 98288.84 432514.43 98292.14 432514.28 98302.57 432515.14 98305.78 432515.1 98312.68 432514.5 98315.79 432514.76 98330.24 432516.47 98350.2 432518.9 98359.02 432518.98 98369.54 432519.34 98375.25 432519.06 98379.239 432518.643 98379.65 432518.6 98384.75 432517.83 98394.82 432514.6 98398.61 432513.64 98402.31 432512.99 98407.52 432513.02 98415.137 432511.415 98416.11 432511.21 98417.881 432511.095 98420.11 432510.95 98421.667 432510.879 98466.26 432508.83 98488.21 432509.63 98500.059 432508.498 98540.83 432504.84 98553.474 432503.705 98573.661 432500.0 98654.366 432485.188 98663.15 432491.014 98668.226 432495.729 98668.482 432495.967 98669.896 432498.043 98670.667 432499.282 98669.978 432500.0 98655.26 432515.34 98657.133 432521.638 98657.16 432521.73 98658.05 432524.75 98660.1 432533.21 98665.27 432554.17 98665.582 432555.616 98667.85 432566.1 98671.004 432579.145 98671.02 432579.21 98671.52 432581.26 98672.6 432585.73 98674.245 432596.305 98674.26 432596.4 98674.47 432597.72 98675.88 432607.77 98678.24 432624.38 98680.34 432639.79 98680.632 432641.638 98680.65 432641.75 98681.189 432645.17 98681.268 432645.668 98684.43 432665.73 98685.376 432671.297 98685.39 432671.38 98685.421 432671.561 98685.433 432671.63 98686.614 432678.524 98686.63 432678.62 98686.643 432678.693 98686.647 432678.717 98690.14 432699.19 98691.41 432705.16 98691.627 432706.104 98691.64 432706.16 98699.39 432740.46 98704.11 432761.54 98712.21 432795.58 98718.75 432823.722 98718.78 432823.85 98718.98 432824.7 98720.59 432831.72 98722.1 432840.09 98724.83 432856.08 98729.16 432884.83 98730.281 432892.855 98732.006 432905.198 98738.48 432951.53 98739.93 432961.86 98742.93 432979.43 98745.43 432989.94 98747.86 432999.64 98747.952 433000.0 98749.86 433007.49 98750.855 433011.928 98754.52 433028.27 98756.93 433039.72 98758.29 433048.33 98762.44 433084.82 98763.14 433090.1 98763.83 433094.04 98765.26 433101.61 98766.04 433105.02 98766.845 433107.684 98766.88 433107.8 98768.57 433110.7 98768.722 433111.139 98768.726 433111.15 98768.925 433111.726 98769.49 433113.36 98776.92 433134.63 98779.78 433143.52 98780.258 433144.981 98782.902 433154.079 98784.624 433159.325 98785.708 433164.005 98786.463 433167.746 98787.158 433171.357 98787.49 433173.881 98787.886 433176.648 98788.565 433182.41 98790.04 433193.213 98790.996 433205.931 98791.673 433207.234 98792.805 433216.067 98793.497 433221.1 98797.133 433239.465 98799.242 433250.0 98800.216 433254.863 98800.494 433256.346 98800.498 433256.366 98800.504 433256.401 98802.06 433264.695 98802.625 433267.395 98805.541 433281.335 98806.514 433285.324 98807.197 433285.358 98809.743 433293.827 98813.147 433306.137 98822.877 433340.228 98827.502 433355.654 98833.108 433374.413 98836.443 433386.027 98837.369 433389.254 98837.879 433390.887 98838.589 433393.158 98841.969 433404.524 98843.399 433409.331 98844.37 433412.509 98848.218 433425.115 98848.542 433426.177 98852.153 433437.009 98853.727 433442.144 98854.752 433446.275 98856.439 433452.387 98856.999 433454.923 98858.326 433461.023 98859.661 433467.53 98862.788 433483.267 98863.534 433486.692 98864.224 433490.928 98865.374 433497.711 98865.735 433500.0 98866.702 433506.132 98867.897 433513.816 98868.841 433519.479 98869.297 433523.582 98870.231 433530.454 98870.758 433535.39 98871.791 433542.2 98873.162 433553.967 98874.321 433560.605 98875.319 433566.143 98876.493 433573.399 98876.691 433574.626 98877.566 433579.609 98877.57 433579.624 98878.381 433582.978 98879.458 433587.159 98880.244 433590.174 98879.803 433590.29 98883.92 433608.564 98886.963 433622.071 98887.232 433623.237 98888.101 433626.999 98889.358 433632.442 98890.358 433632.534 98893.462 433647.006 98893.797 433648.565 98894.608 433652.08 98894.904 433653.36 98896.889 433661.959 98897.434 433664.32 98898.145 433667.225 98899.433 433672.485 98900.721 433677.749 98901.657 433681.572 98901.99 433682.982 98903.233 433688.235 98904.93 433695.411 98906.322 433701.295 98910.232 433716.723 98910.724 433718.377 98912.383 433723.96 98916.143 433735.912 98918.334 433743.592 98919.219 433746.693 98920.039 433750.0 98921.84 433757.258 98921.85 433757.3 98922.865 433761.766 98922.87 433761.79 98923.866 433765.952 98923.87 433765.97 98924.849 433770.112 98924.86 433770.16 98925.551 433774.343 98925.56 433774.4 98926.169 433778.657 98926.866 433782.884 98926.87 433782.91 98927.576 433787.141 98927.58 433787.16 98928.09 433791.45 98928.598 433795.72 98929.11 433800.02 98929.112 433800.031 98929.81 433804.27 98930.65 433810.25 98931.75 433817.04 98931.76 433817.078 98932.86 433821.22 98932.863 433821.233 98933.756 433825.402 98933.76 433825.42 98934.77 433829.6 98936.06 433833.71 98936.07 433833.742 98937.36 433837.83 98937.364 433837.848 98938.46 433841.99 98938.481 433842.054 98940.637 433848.54 98940.64 433848.55 98942.646 433854.035 98945.153 433860.494 98945.75 433862.03 98945.758 433862.053 98947.23 433866.42 98947.245 433866.467 98948.52 433870.55 98948.536 433870.616 98949.321 433873.897 98949.52 433874.73 98949.539 433874.845 98950.23 433878.99 98950.25 433879.131 98950.84 433883.27 98950.851 433883.339 98951.53 433887.48 98951.54 433887.54 98952.35 433891.77 98952.798 433897.823 98952.8 433897.85 98953.787 433904.681 98953.79 433904.7 98953.91 433909.06 98954.52 433913.31 98954.525 433913.365 98954.932 433917.54 98954.94 433917.62 98955.97 433922.1 98956.292 433923.434 98957.4 433928.024 98958.199 433930.357 98959.159 433932.899 98960.689 433936.347 98962.219 433940.688 98962.23 433940.72 98963.705 433944.775 98965.407 433948.84 98966.879 433952.859 98966.89 433952.89 98969.22 433959.38 98969.236 433959.421 98969.591 433960.32 98971.446 433965.03 98971.47 433965.09 98972.96 433969.17 98973.915 433972.472 98974.16 433973.32 98974.181 433973.386 98975.46 433977.45 98975.468 433977.475 98976.766 433981.601 98977.86 433985.74 98977.869 433985.779 98978.854 433989.887 98979.87 433994.13 98979.873 433994.143 98980.82 433998.32 98980.836 433998.435 98981.054 434000.0 98981.449 434002.831 98981.46 434002.91 98982.48 434008.857 98983.133 434014.931 98983.63 434020.944 98983.988 434027.809 98984.103 434033.751 98984.113 434034.256 98984.13 434035.12 98984.142 434035.756 98984.285 434038.772 98987.149 434053.745 98991.591 434076.118 98994.738 434092.12 98995.484 434095.201 98998.974 434110.457 99000.0 434113.878 99005.173 434131.124 99005.446 434132.036 99008.138 434140.692 99008.481 434142.019 99012.654 434158.175 99018.095 434178.956 99018.394 434180.616 99018.713 434184.164 99019.101 434188.488 99019.287 434189.255 99020.789 434195.434 99022.48 434202.396 99023.192 434205.325 99023.968 434208.517 99024.653 434211.338 99032.874 434250.0 99044.029 434302.46 99044.811 434306.14 99044.993 434306.996 99046.299 434313.07 99049.48 434327.865 99049.869 434329.673 99066.681 434406.523 99073.035 434445.704 99073.66 434449.554 99073.743 434450.067 99073.977 434451.237 99083.734 434500.0 99091.481 434538.72 99099.922 434577.712 99106.714 434608.637 99112.717 434637.99 99113.464 434641.642 99114.582 434647.109 99115.935 434653.723 99120.894 434678.616 99121.321 434680.759 99121.347 434680.892 99124.834 434699.594 99126.523 434708.608 99139.698 434705.667 99143.264 434711.224 99144.873 434716.263 99146.45 434721.321 99147.658 434726.514 99148.782 434731.515 99149.998 434736.506 99150.046 434737.135 99151.464 434743.439 99152.978 434750.0 99154.101 434755.166 99155.352 434760.972 99156.621 434766.297 99157.822 434771.716 99158.585 434775.107 99159.076 434777.185 99160.121 434781.936 99160.374 434782.712 99160.301 434782.728 99161.547 434788.196 99162.761 434793.554 99163.876 434798.638 99165.109 434804.121 99165.345 434805.17 99164.652 434805.326 99166.409 434812.395 99167.71 434813.14 99168.247 434815.003 99170.106 434821.805 99171.743 434827.048 99172.827 434830.869 99173.06 434831.671 99174.671 434838.737 99175.477 434851.72 99175.694 434855.219 99176.373 434855.725 99176.371 434856.889 99176.369 434857.862 99175.821 434858.322 99176.299 434863.695 99176.051 434866.53 99175.515 434872.641 99174.589 434884.669 99174.417 434886.908 99174.139 434890.511 99174.068 434891.43 99176.777 434892.069 99177.108 434892.113 99176.856 434893.878 99176.567 434895.903 99175.052 434896.265 99171.756 434961.214 99171.733 434961.736 99171.576 434965.305 99170.845 434977.327 99169.188 435000.0 99165.815 435046.154 99164.739 435060.886 99168.161 435082.486 99168.27 435083.174 99170.027 435094.26 99173.24 435114.54 99174.042 435119.604 99194.703 435250.0 99196.047 435258.484 99000.0 435337.341 98962.655 435352.362 98854.519 435381.325 98725.233 435410.54 98717.477 435411.957 98582.074 435436.69 98544.146 435443.136 98541.845 435443.527 98385.571 435469.935 98275.199 435488.586 98195.833 435500.0 98000.0 435528.164 97738.64 435565.751 97737.633 435565.896 97713.236 435566.089 97686.901 435565.205 97669.741 435564.629 97334.155 435485.487 97379.903 435680.224 97382.133 435690.371 97384.055 435699.186 97389.056 435720.464 97393.86 435741.389 97399.388 435764.551 97402.049 435776.355 97403.641 435783.415 97404.941 435788.883 97408.049 435801.955 97409.522 435808.74 97410.563 435813.533 97413.879 435827.075 97415.424 435833.908 97416.917 435840.839 97420.18 435854.656 97422.33 435863.124 97425.366 435875.034 97427.89 435885.84 97428.223 435887.268 97430.33 435897.288 97432.339 435907.081 97434.655 435920.585 97435.691 435923.855 97437.422 435925.719 97472.448 435945.1 97471.873 435946.402 97472.755 435946.777 97469.478 435953.19 97469.259 435953.618 97460.241 435971.27 97456.902 435977.805 97461.953 436000.0 97464.943 436013.141 97476.404 436010.267 97485.815 436014.966 97492.049 436018.079 97492.56 436018.334 97500.0 436022.049 97511.636 436027.859 97512.027 436027.743 97515.943 436026.581 97521.381 436028.983 97522.611 436033.489 97524.61 436034.352 97524.641 436034.365 97524.743 436035.241 97527.767 436036.742 97557.043 436051.328 97572.094 436058.747 97572.478 436058.938 97575.937 436060.741 97579.92 436062.388 97582.172 436063.296 97602.445 436071.474 97606.667 436072.711 97627.78 436078.897 97629.101 436079.219 97642.718 436082.533 97675.527 436089.908 97699.303 436095.235 97727.312 436101.046 97730.64 436101.661 97736.313 436102.71 97740.319 436103.45 97764.55 436107.928 97775.848 436110.081 97810.359 436103.222 97840.091 436097.313 97840.573 436097.199 97863.443 436091.773 97862.989 436100.486 97862.54 436109.112 97861.31 436139.414 97860.849 436149.274 97861.634 436163.19 97861.681 436164.59 97861.671 436165.164 97861.87 436175.929 97861.811 436178.536 97861.529 436186.224 97861.19 436195.451 97861.036 436199.88 97860.996 436201.026 97860.863 436204.858 97859.759 436231.76 97859.209 436250.0 97858.825 436262.717 97858.627 436268.119 97858.541 436270.475 97857.584 436292.697 97856.434 436322.602 97855.269 436358.989 97853.959 436390.722 97852.768 436425.28 97852.591 436430.412 97851.701 436438.466 97850.469 436449.621 97850.394 436450.305 97850.182 436457.699 97850.13 436459.498 97849.244 436490.367 97848.968 436500.0 97848.962 436500.193 97848.84 436503.844 97848.612 436510.71 97848.38 436517.663 97848.15 436524.557 97847.92 436531.477 97847.689 436538.401 97847.554 436542.433 97847.526 436543.284 97847.4 436547.078 97847.283 436550.574 97847.106 436555.889 97847.1 436556.083 97846.983 436559.63 97846.187 436583.479 97828.672 436579.839 97828.563 436579.816 97823.954 436578.858 97823.804 436578.827 97814.128 436579.087 97799.65 436579.477 97785.192 436579.866 97770.789 436580.254 97756.452 436580.64 97742.12 436581.026 97727.401 436581.474 97713.323 436581.812 97698.177 436582.209 97679.143 436582.721 97676.157 436582.801 97676.435 436585.741 97676.52 436586.641 97676.584 436587.316 97676.78 436589.39 97691.954 436750.0 97694.276 436774.582 97671.948 436753.76 97672.244 436755.943 97673.023 436761.679 97674.294 436771.197 97674.308 436771.302 97674.619 436773.657 97675.193 436777.825 97675.203 436777.899 97676.502 436787.331 97675.668 436786.739 97672.605 436784.27 97660.97 436773.785 97652.094 436765.423 97645.733 436759.811 97637.927 436752.888 97635.855 436750.995 97634.655 436750.0 97626.65 436743.36 97617.03 436735.364 97612.84 436731.886 97605.535 436725.812 97588.48 436711.63 97578.819 436704.254 97553.442 436685.015 97549.41 436681.8 97527.318 436665.342 97525.69 436664.116 97525.544 436664.007 97509.14 436651.77 97500.0 436645.204 97497.528 436643.428 97474.449 436627.413 97465.912 436621.645 97465.906 436621.641 97462.748 436619.508 97438.77 436603.629 97417.366 436590.046 97410.816 436562.698 97409.406 436556.813 97409.405 436556.809 97395.8 436500.0 97335.925 436250.0 97278.518 436010.305 97278.185 436008.915 97276.05 436000.0 97273.574 435989.7 97273.509 435989.429 97272.709 435986.103 97270.229 435975.027 97267.273 435963.576 97264.714 435952.432 97259.13 435928.574 97251.419 435896.785 97239.708 435894.842 97239.433 435894.796 97237.078 435894.406 97235.851 435891.231 97234.323 435887.206 97232.693 435883.708 97231.97 435882.156 97228.325 435872.335 97225.384 435864.232 97225.237 435863.9 97224.957 435863.269 97224.165 435861.481 97219.7 435851.408 97215.043 435841.001 97214.323 435839.391 97211.713 435833.558 97211.397 435832.264 97213.39 435833.064 97221.477 435830.681 97219.375 435820.965 97218.289 435813.958 97216.065 435799.615 97215.402 435794.826 97214.145 435785.748 97213.512 435776.459 97213.544 435775.662 97213.868 435767.728 97213.902 435765.992 97213.904 435765.973 97214.083 435764.265 97214.089 435764.229 97214.399 435762.602 97214.421 435762.509 97214.866 435760.921 97214.887 435760.855 97215.465 435759.304 97215.5 435759.221 97216.225 435757.685 97216.235 435757.668 97217.098 435756.181 97217.11 435756.164 97218.094 435754.756 97230.96 435741.663 97230.478 435740.998 97229.985 435740.341 97229.48 435739.694 97228.964 435739.055 97228.436 435738.426 97227.898 435737.805 97227.348 435737.195 97226.789 435736.594 97226.218 435736.003 97225.637 435735.422 97225.047 435734.852 97224.446 435734.292 97223.835 435733.743 97223.215 435733.204 97222.586 435732.677 97221.947 435732.161 97221.299 435731.656 97220.643 435731.162 97220.606 435731.135 97219.106 435730.022 97217.597 435728.922 97217.213 435728.647 97217.092 435728.558 97216.917 435728.436 97216.078 435727.835 97214.551 435726.76 97213.682 435726.16 97213.527 435726.051 97213.401 435725.966 97213.014 435725.699 97211.468 435724.65 97209.914 435723.615 97208.351 435722.593 97206.779 435721.584 97206.399 435721.345 97206.253 435721.25 97206.064 435721.134 97205.199 435720.588 97203.61 435719.606 97202.736 435719.075 97202.547 435718.958 97202.412 435718.879 97202.014 435718.637 97200.409 435717.682 97198.796 435716.74 97173.758 435702.444 97157.64 435694.036 97155.659 435693.031 97155.428 435692.914 97153.675 435692.033 97153.211 435691.802 97151.687 435691.043 97150.988 435690.698 97149.971 435690.196 97148.763 435689.604 97148.253 435689.354 97146.532 435688.518 97146.117 435688.317 97145.701 435688.116 97141.545 435686.027 97133.625 435682.047 97133.35 435681.903 97133.074 435681.759 97131.887 435681.143 97130.142 435680.251 97129.488 435679.921 97128.834 435679.592 97128.656 435679.504 97128.178 435679.266 97127.717 435679.04 97127.166 435678.767 97126.728 435678.554 97126.348 435678.367 97126.056 435678.226 97125.671 435678.039 97124.511 435677.483 97124.17 435677.322 97122.667 435676.613 97122.386 435670.9 97122.376 435670.701 97121.882 435670.725 97109.722 435671.32 97105.487 435669.574 97104.901 435669.34 97104.704 435669.265 97104.574 435669.218 97104.31 435669.12 97104.052 435669.029 97103.913 435668.979 97103.842 435668.956 97103.714 435668.911 97103.114 435668.716 97102.51 435668.534 97102.4 435668.503 97102.308 435668.476 97102.18 435668.442 97101.903 435668.364 97101.495 435668.259 97101.419 435668.241 97101.291 435668.208 97100.677 435668.065 97090.787 435666.257 97036.139 435655.476 97026.87 435653.646 97003.646 435649.062 97001.61 435648.66 96976.488 435641.124 96975.53 435644.319 96933.319 435638.991 96930.815 435642.156 96924.816 435657.157 96918.632 435655.653 96847.112 435638.267 96836.079 435664.261 96825.964 435687.763 96816.318 435710.816 96809.446 435727.006 96800.922 435747.089 96709.705 435961.995 96711.813 435965.503 96712.821 435967.514 96713.467 435968.804 96715.421 435972.982 96717.05 435977.234 96718.33 435981.636 96719.589 435991.016 96719.565 435992.892 96719.091 435998.641 96718.979 436000.0 96657.85 436135.446 96659.901 436237.886 96654.367 436249.468 96653.402 436251.487 96652.467 436252.604 96626.941 436283.105 96611.178 436279.226 96600.0 436276.476 96592.772 436274.697 96592.7 436274.679 96590.983 436274.257 96564.848 436345.678 96558.653 436363.425 96555.93 436370.814 96552.291 436380.691 96552.166 436381.03 96547.208 436394.8 96545.287 436400.135 96542.0 436409.328 96541.032 436412.037 96537.417 436422.147 96530.011 436444.102 96527.132 436452.635 96524.705 436459.83 96523.808 436465.931 96521.739 436479.999 96521.611 436480.872 96518.699 436500.0 96517.811 436504.793 96516.636 436513.525 96516.282 436516.118 96515.844 436519.333 96515.735 436520.129 96515.034 436523.381 96515.458 436530.235 96515.62 436531.432 96516.353 436536.85 96516.531 436538.165 96517.854 436546.269 96520.152 436555.324 96522.776 436564.028 96526.25 436571.856 96530.222 436579.109 96534.921 436585.186 96540.544 436591.086 96545.318 436595.161 96549.892 436598.511 96555.916 436601.91 96564.115 436605.358 96570.999 436607.437 96571.14 436607.48 96571.156 436607.485 96572.963 436608.03 96578.087 436608.904 96589.719 436609.985 96589.633 436626.058 96589.666 436658.195 96614.982 436657.571 96639.979 436657.104 96664.959 436656.055 96689.909 436653.925 96721.245 436649.682 96729.286 436648.255 96732.533 436647.679 96747.804 436644.968 96734.079 436680.0 96697.461 436773.46 96688.317 436796.799 96688.199 436797.101 96684.331 436806.974 96684.164 436807.401 96669.378 436845.138 96668.616 436847.118 96641.795 436915.671 96633.543 436936.665 96618.351 436975.318 96608.688 437000.0 96604.774 437009.999 96586.994 437055.416 96580.292 437072.534 96569.342 437100.506 96553.661 437140.561 96549.164 437152.047 96550.019 437151.753 96600.0 437144.932 96647.369 437138.655 96655.972 437137.667 96741.73 437108.351 96790.759 437091.511 96951.164 437091.457 97000.0 437091.587 97216.909 437092.164 97338.978 437092.489 97454.738 437092.6 97547.82 437103.78 97649.92 437130.21 97726.91 437156.78 97841.6 437196.97 97881.34 437151.52 97889.14 437158.335 97890.734 437159.727 97937.99 437201.01 98000.0 437233.801 98030.634 437250.0 98034.0 437251.78 98064.88 437276.32 98067.8 437278.74 98112.43 437313.87 98113.32 437314.57 98133.45 437330.96 98164.732 437347.286 98168.517 437349.261 98168.773 437349.395 98189.182 437360.046 98193.869 437362.492 98194.807 437362.982 98198.556 437364.938 98201.05 437366.24 98200.298 437370.765 98197.875 437385.355 98195.6 437399.05 98196.511 437399.522 98196.597 437399.566 98221.39 437412.4 98245.06 437425.19 98274.55 437442.76 98297.61 437457.4 98319.6 437472.7 98342.54 437490.18 98347.642 437494.592 98347.652 437494.601 98352.053 437498.406 98352.077 437498.427 98353.896 437500.0 98355.33 437501.24 98377.68 437522.17 98399.49 437545.2 98411.51 437559.47 98417.448 437567.364 98424.314 437576.492 98426.746 437579.726 98432.24 437587.03 98436.221 437593.021 98443.61 437604.14 98449.235 437613.087 98449.305 437613.199 98450.89 437615.72 98453.258 437619.758 98453.472 437620.123 98460.57 437632.23 98470.45 437650.88 98485.01 437683.71 98488.428 437693.118 98490.922 437699.984 98492.08 437703.17 98492.434 437704.102 98492.752 437704.938 98495.203 437711.389 98495.505 437712.183 98495.54 437712.276 98497.64 437717.8 98498.77 437720.75 98497.8 437724.48 98492.265 437749.536 98491.228 437754.23 98490.122 437759.234 98492.14 437772.01 98494.686 437774.373 98496.75 437776.289 98498.96 437778.34 98507.35 437785.99 98509.28 437787.15 98511.14 437787.59 98513.15 437787.19 98514.53 437786.71 98513.971 437789.847 98513.768 437790.986 98513.145 437794.48 98512.83 437796.25 98530.45 437811.388 98527.38 437822.33 98527.243 437822.777 98526.57 437824.98 98525.367 437828.639 98525.345 437828.706 98525.1 437829.45 98514.481 437868.391 98514.19 437869.46 98514.17 437869.937 98514.058 437872.588 98513.668 437881.859 98513.147 437894.211 98512.7 437904.81 98506.92 437945.98 98506.917 437945.998 98506.913 437946.027 98505.45 437956.47 98502.353 437971.311 98498.64 437989.1 98497.27 437995.063 98496.135 438000.0 98483.5 438054.97 98464.46 438128.22 98459.688 438146.578 98459.49 438147.34 98455.85 438161.365 98455.794 438161.582 98455.284 438163.544 98453.337 438171.031 98457.916 438174.525 98459.86 438176.04 98448.36 438191.38 98429.348 438217.353 98450.239 438233.05 98463.095 438242.71 98469.992 438247.893 98469.466 438250.0 98456.98 438300.0 98411.049 438483.973 98410.696 438485.389 98409.51 438490.139 98407.048 438500.0 98405.993 438504.227 98405.615 438505.741 98404.899 438508.611 98404.898 438508.614 98399.11 438531.797 98375.103 438627.954 98372.706 438637.554 98368.351 438654.999 98368.314 438655.145 98368.302 438655.193 98368.29 438655.242 98368.266 438655.339 98368.169 438655.727 98367.927 438656.697 98367.653 438657.793 98361.686 438681.694 98361.347 438683.052 98354.381 438710.956 98344.634 438750.0 98337.306 438779.351 98299.67 438930.1 98296.187 438944.052 98282.218 439000.0 98219.8 439250.0 98196.69 439342.56 98192.253 439360.33 98163.138 439476.939 98161.2 439484.7 98172.66 439487.4 98213.004 439496.912 98226.101 439500.0 98340.337 439526.934 98371.612 439534.308 98373.656 439534.79 98500.0 439564.578 98624.237 439593.87 98636.433 439596.745 98844.15 439645.72 98902.221 439687.149 98924.926 439703.347 98924.93 439703.35 98990.319 439750.0 99000.0 439756.906 99002.221 439758.491 99000.0 439761.579 98996.41 439766.57 98972.01 439800.51 98958.019 439819.967 98940.94 439843.72 98936.33 439850.13 98934.827 439852.224 98932.629 439855.34 98928.669 439860.804 98924.745 439866.598 98920.334 439872.387 98916.383 439877.953 98912.273 439883.597 98911.58 439884.56 98909.0 439888.15 98889.03 439916.41 98885.09 439921.98 98880.99 439927.79 98876.98 439933.46 98872.99 439939.11 98868.98 439944.77 98864.99 439950.42 98861.0 439956.07 98857.0 439961.73 98852.95 439967.46 98849.01 439973.04 98845.87 439977.48 98829.953 440000.0 98825.5 440006.3 98796.504 440046.372 98795.688 440047.5 98786.272 440060.513 98771.874 440080.411 98757.7 440100.0 98684.014 440202.803 98681.68 440206.06 98679.53 440209.059 98664.75 440229.681 98663.957 440230.787 98650.186 440250.0 98623.771 440286.856 98607.068 440310.16 98605.014 440313.025 98592.484 440330.507 98587.73 440337.14 98584.15 440342.135 98517.313 440435.388 98505.3 440452.15 98500.0 440459.615 98493.33 440469.01 98480.859 440486.57 98476.571 440492.608 98471.321 440500.0 98468.09 440504.55 98460.055 440515.811 98455.195 440522.711 98450.558 440529.242 98443.21 440539.59 98442.081 440541.177 98437.92 440547.03 98426.907 440562.538 98421.36 440570.35 98417.993 440575.091 98415.21 440579.01 98412.199 440583.249 98404.778 440613.077 98403.55 440618.17 98398.193 440639.663 98384.96 440692.75 98371.783 440745.702 98371.766 440745.769 98370.714 440750.0 98369.096 440756.506 98359.882 440793.546 98357.242 440804.159 98355.198 440812.48 98352.75 440822.44 98346.408 440848.251 98346.324 440848.594 98345.785 440850.789 98345.77 440850.85 98343.678 440859.179 98319.546 440955.242 98316.499 440967.37 98308.83 440997.9 98308.299 441000.0 98305.475 441011.167 98304.744 441014.058 98304.303 441015.802 98304.0 441017.0 98303.177 441020.135 98255.336 441202.384 98248.268 441229.309 98248.234 441229.437 98248.067 441230.074 98246.821 441234.82 98249.38 441235.455 98263.32 441238.91 98308.054 441250.0 98322.286 441253.528 98339.8 441257.87 98329.22 441301.02 98315.268 441356.85 98313.823 441362.633 98284.19 441482.14 98294.562 441489.579 98309.093 441500.0 98327.129 441512.934 98350.487 441529.686 98353.48 441531.833 98356.164 441533.758 98365.133 441540.19 98411.526 441573.462 98412.22 441573.96 98412.493 441574.156 98437.247 441591.906 98444.56 441597.15 98449.804 441589.837 98469.618 441562.208 98473.314 441557.054 98500.0 441519.844 98509.406 441506.728 98510.776 441504.817 98514.23 441500.0 98538.29 441466.45 98544.651 441457.579 98585.521 441400.587 98590.217 441394.039 98608.89 441368.0 98621.281 441373.434 98630.027 441377.27 98638.826 441381.0 98649.551 441385.832 98663.943 441392.143 98664.109 441392.216 98669.697 441394.667 98672.183 441395.757 98684.23 441401.04 98688.315 441395.361 98692.82 441389.1 98709.412 441399.368 98735.875 441415.745 98774.97 441439.94 98856.59 441494.27 98859.22 441496.133 98864.682 441500.0 98865.035 441500.25 98885.209 441514.562 98890.73 441518.44 98937.03 441551.22 99000.0 441597.625 99016.86 441610.05 99096.63 441669.94 99121.57 441688.71 99143.14 441704.94 99143.391 441705.129 99176.53 441730.08 99202.992 441750.0 99216.318 441760.032 99256.42 441790.22 99286.956 441813.2 99290.77 441816.07 99500.0 441973.556 99524.261 441991.817 99535.132 442000.0 99542.28 442005.38 99549.935 442011.143 99596.452 442046.162 99633.63 442074.15 99626.239 442084.46 99787.429 442201.568 99789.249 442202.89 99800.109 442187.763 99806.855 442192.554 99807.52 442193.027 99822.424 442203.611 99821.166 442205.47 99819.224 442209.883 99817.827 442213.121 99818.088 442215.56 99818.467 442217.962 99820.543 442220.61 99822.987 442222.415 99850.823 442242.177 99858.193 442250.0 99859.651 442251.548 99864.459 442259.898 99864.896 442260.658 99869.259 442268.235 99881.233 442293.234 99885.247 442298.892 99888.683 442302.469 99896.247 442310.052 99905.312 442316.579 99978.16 442367.797 100000.0 442383.152 100021.31 442398.135 100022.416 442398.913 100110.695 442462.246 100163.319 442500.0 100200.073 442526.368 100205.197 442530.044 100218.392 442540.542 100221.002 442541.21 100222.861 442541.135 100226.908 442540.544 100229.345 442538.941 100230.829 442537.407 100249.349 442511.909 100251.496 442508.943 100253.526 442506.509 100255.627 442504.563 100257.431 442503.018 100259.71 442502.044 100261.901 442501.439 100263.914 442501.198 100266.314 442501.524 100268.255 442502.005 100270.248 442502.711 100274.573 442504.625 100285.851 442509.474 100315.184 442522.567 100324.382 442526.673 100333.58 442530.779 100377.171 442550.236 100414.128 442570.808 100431.871 442580.684 100535.706 442641.467 100547.376 442648.298 100627.121 442694.286 100657.889 442711.277 100660.232 442712.571 100662.689 442714.01 100675.039 442721.244 100680.491 442724.438 100714.001 442747.317 100751.741 442773.733 100760.829 442780.094 100769.697 442786.238 100843.346 442837.261 100866.264 442852.35 100873.62 442856.408 100877.424 442857.957 100885.086 442860.873 100894.863 442863.899 100916.033 442870.386 100941.196 442877.186 100966.389 442887.261 100979.726 442895.017 101000.0 442906.807 101009.753 442912.48 101022.544 442921.282 101023.059 442921.606 101031.71 442927.041 101032.596 442927.685 101025.605 442932.601 101028.521 442934.594 101000.0 442954.639 100978.638 442969.652 100937.113 443000.0 100931.61 443004.022 100863.945 443053.474 100785.482 443108.09 100718.893 443156.676 100604.976 443235.591 100571.201 443259.689 100567.69 443262.194 100526.302 443294.363 100512.928 443307.886 100508.723 443313.177 100501.856 443323.228 100498.873 443329.352 100496.48 443336.128 100495.048 443341.531 100493.545 443349.239 100493.369 443350.257 100487.533 443352.238 100484.883 443363.131 100483.05 443372.988 100482.876 443374.918 100482.61 443377.873 100483.305 443387.724 100484.174 443397.473 100485.603 443407.941 100487.83 443420.301 100491.198 443433.923 100494.287 443445.046 100495.062 443447.139 100500.854 443456.278 100506.552 443463.691 100506.554 443463.694 100509.753 443467.856 100514.934 443474.099 100520.722 443482.913 100522.1 443485.173 100522.904 443491.544 100521.953 443497.762 100521.491 443500.0 100520.18 443506.342 100516.365 443523.93 100516.118 443525.783 100515.608 443529.62 100514.832 443535.66 100514.727 443541.492 100515.628 443547.712 100518.555 443559.963 100526.973 443591.667 100535.994 443621.486 100540.234 443634.797 100544.735 443648.856 100550.986 443669.024 100558.246 443694.011 100565.995 443726.248 100575.791 443766.342 100586.316 443810.883 100597.137 443856.971 100628.924 443989.529 100629.204 443990.698 100629.56 443992.184 100630.25 443996.408 100631.464 444000.0 100632.213 444002.214 100634.433 444007.882 100643.959 444029.38 100658.284 444055.882 100665.478 444069.912 100668.6 444076.0 100675.691 444089.83 100683.681 444108.87 100684.29 444110.678 100686.743 444117.967 100696.303 444153.263 100729.442 444275.614 100735.626 444298.446 100738.136 444307.714 100743.538 444327.658 100755.043 444370.131 100790.642 444500.0 100804.162 444549.321 100806.666 444558.457 100815.961 444593.47 100821.683 444615.024 100822.834 444621.259 100825.495 444635.672 100829.623 444653.474 100834.129 444678.537 100836.35 444690.89 100837.06 444690.892 100838.138 444690.895 100838.236 444690.895 100843.15 444690.907 100842.754 444708.497 100842.753 444708.536 100842.74 444709.12 100842.68 444711.78 100839.98 444730.47 100837.28 444741.18 100829.35 444762.38 100817.378 444790.746 100802.28 444826.52 100757.42 444927.88 100725.696 445000.0 100705.69 445045.48 100680.12 445099.92 100659.24 445146.21 100635.25 445201.22 100602.84 445274.35 100591.88 445299.53 100552.538 445386.92 100548.253 445396.44 100548.21 445396.535 100542.157 445393.812 100541.95 445393.719 100535.96 445391.025 100535.64 445390.881 100533.701 445390.009 100515.451 445381.802 100492.724 445371.582 100444.5 445349.896 100443.791 445349.577 100441.728 445348.649 100437.878 445346.918 100432.508 445344.503 100429.239 445343.033 100427.446 445342.227 100427.343 445342.181 100427.316 445342.169 100404.231 445331.788 100403.26 445331.351 100339.053 445302.478 100158.505 445221.287 100136.731 445211.495 100000.0 445150.008 99895.717 445103.113 99890.867 445100.932 99789.759 445055.465 99666.42 445000.0 99619.109 444978.725 99537.398 444941.98 99482.929 444917.402 99516.7 444844.728 99522.857 444831.48 99526.533 444823.569 99527.235 444822.06 99534.788 444805.805 99536.64 444801.82 99467.0 444770.56 99503.28 444689.73 99517.057 444659.04 99526.533 444637.932 99528.959 444632.528 99531.32 444627.27 99487.603 444602.209 99485.11 444600.78 99464.697 444589.079 99431.611 444570.115 99398.35 444551.05 99367.12 444533.15 99398.65 444478.14 99419.71 444438.431 99438.018 444397.381 99453.49 444355.18 99466.055 444312.021 99475.653 444268.106 99482.24 444223.64 99484.465 444201.101 99485.909 444178.499 99486.57 444155.86 99486.447 444133.21 99485.54 444110.577 99483.85 444087.99 99476.78 444010.06 99476.56 444007.61 99472.37 443961.35 99470.93 443945.55 99088.4 443800.45 98871.41 443718.14 98883.42 443691.09 99092.534 443220.094 99092.54 443220.08 99112.125 443234.117 99113.79 443235.31 99120.9 443240.33 99145.67 443212.07 99353.631 443394.321 99363.78 443403.21 99383.56 443376.09 99386.5 443372.06 99392.72 443363.3 99395.875 443358.925 99437.737 443395.575 99439.584 443392.994 99441.418 443390.431 99441.599 443390.177 99447.21 443382.335 99448.905 443379.965 99449.492 443379.143 99449.722 443378.823 99450.475 443377.771 99451.465 443376.387 99467.155 443354.456 99469.9 443350.62 99439.59 443324.11 99438.269 443322.955 99410.462 443298.645 99410.257 443298.466 99409.3 443297.63 99408.503 443296.933 99406.634 443295.298 99396.944 443286.824 99396.152 443286.132 99387.421 443278.496 99385.132 443276.494 99379.273 443271.37 99379.01 443271.14 99375.695 443268.241 99373.808 443266.591 99365.225 443259.085 99363.712 443257.762 99361.826 443256.112 99354.837 443250.0 99351.203 443246.822 99349.106 443244.988 99348.73 443244.66 99318.803 443218.487 99318.44 443218.17 99288.702 443192.164 99288.16 443191.69 99273.02 443178.44 99258.231 443165.488 99257.89 443165.19 99246.881 443155.607 99240.818 443150.329 99227.62 443138.84 99221.18 443133.23 99195.941 443114.713 99195.65 443114.5 99129.09 443056.22 99131.26 443053.18 99132.792 443051.053 99147.72 443030.32 99146.657 443029.564 99114.44 443006.65 99103.51 443000.0 99080.999 442986.303 99044.436 442964.055 99011.23 442943.85 99000.0 442935.877 98977.82 442920.13 98970.688 442930.0 98957.06 442948.86 98909.728 442943.576 98865.79 442938.67 98827.95 442921.62 98791.118 442905.01 98753.32 442887.95 98713.17 442874.51 98698.558 442869.62 98696.369 442868.887 98673.127 442861.109 98635.27 442848.44 98620.085 442854.44 98553.72 442880.66 98548.54 442865.84 98528.62 442859.2 98509.93 442866.914 98509.723 442866.999 98500.0 442871.012 98456.92 442888.79 98451.64 442896.35 98444.33 442906.82 98416.692 442915.2 98380.25 442926.25 98390.76 442911.08 98358.19 442888.4 98349.08 442901.07 98334.49 442921.37 98325.46 442933.94 98307.9 442922.46 98303.512 442919.59 98302.29 442918.791 98293.651 442913.141 98291.86 442911.97 98291.593 442911.795 98285.58 442907.86 98285.267 442907.655 98269.66 442897.45 98269.505 442897.349 98258.26 442890.0 98268.104 442875.947 98236.32 442853.63 98250.39 442834.23 98260.46 442820.34 98271.64 442804.93 98255.74 442794.06 98233.74 442779.02 98250.69 442755.153 98253.64 442751.0 98252.239 442750.0 98241.41 442742.27 98207.81 442733.62 98210.51 442723.81 98210.775 442722.122 98211.86 442715.2 98211.01 442706.16 98208.44 442695.88 98205.53 442690.59 98203.11 442686.5 98194.32 442679.01 98184.49 442673.23 98173.9 442668.89 98125.01 442654.03 98079.44 442640.2 98050.9 442631.53 98041.06 442628.19 98033.4 442625.34 98028.18 442622.74 98000.0 442604.598 97920.62 442553.494 97918.78 442552.31 97916.949 442559.695 97915.986 442563.576 97915.95 442563.723 97915.709 442564.694 97915.1 442567.15 97919.076 442572.631 97920.65 442574.8 97928.1 442581.65 97918.25 442590.25 97913.2 442591.95 97908.7 442591.4 97908.432 442592.628 97907.0 442599.2 97840.15 442593.25 97795.85 442602.6 97782.45 442599.1 97781.521 442598.932 97780.787 442598.885 97778.656 442598.75 97777.013 442598.728 97775.512 442598.827 97773.779 442599.006 97772.286 442599.233 97770.607 442599.603 97768.968 442600.065 97767.59 442600.498 97766.193 442601.067 97764.851 442601.684 97762.155 442603.133 97760.805 442604.033 97759.602 442604.895 97758.298 442605.94 97757.063 442607.051 97755.95 442608.2 97744.386 442618.514 97733.75 442628.0 97700.6 442657.9 97653.936 442699.592 97597.516 442750.0 97544.074 442797.748 97542.231 442799.394 97539.836 442801.534 97539.65 442801.7 97500.0 442837.226 97484.18 442851.4 97470.54 442863.619 97467.1 442866.7 97459.45 442874.45 97454.55 442883.9 97452.345 442889.677 97452.249 442889.93 97448.9 442898.7 97442.5 442906.25 97427.15 442921.1 97343.132 442995.022 97341.677 442996.302 97341.35 442996.59 97342.87 442997.75 97341.479 443000.0 97330.24 443018.18 97329.76 443018.957 97328.933 443020.295 97328.22 443021.45 97324.856 443026.892 97314.64 443043.42 97302.03 443042.95 97292.92 443041.68 97278.4 443037.78 97257.73 443030.99 97230.65 443020.72 97190.54 443003.97 97183.153 443000.0 97172.77 442994.42 97158.2 442983.83 97139.36 442969.65 97121.51 442954.75 97109.83 442946.17 97096.92 442937.99 97082.42 442929.19 97061.85 442914.52 97051.53 442904.9 97036.44 442890.01 97013.82 442870.62 97002.85 442862.09 97000.0 442860.17 96992.0 442854.78 96975.1 442845.8 96960.57 442838.49 96938.58 442828.47 96932.28 442824.64 96930.224 442823.295 96869.03 442783.27 96856.38 442774.33 96847.81 442768.07 96814.64 442743.85 96803.97 442736.71 96779.38 442724.57 96772.74 442722.49 96759.72 442720.34 96748.5 442719.69 96735.66 442719.76 96724.63 442720.4 96709.32 442723.13 96687.08 442727.46 96676.79 442730.5 96665.17 442735.08 96653.82 442739.82 96642.44 442743.92 96605.18 442757.61 96588.35 442764.41 96573.24 442771.99 96539.06 442790.68 96529.41 442794.49 96520.91 442796.25 96515.08 442796.51 96507.47 442795.92 96499.99 442793.64 96493.64 442790.52 96487.04 442786.95 96481.65 442782.33 96475.6 442776.18 96471.65 442770.33 96468.43 442763.32 96466.87 442755.95 96465.65 442745.9 96465.54 442735.29 96466.12 442721.19 96468.265 442694.953 96468.35 442693.91 96467.84 442670.18 96468.18 442651.2 96468.01 442635.96 96467.11 442615.81 96466.01 442603.56 96464.73 442594.17 96462.02 442585.4 96458.12 442576.03 96451.81 442565.95 96445.97 442558.6 96440.9 442553.59 96436.02 442549.13 96429.11 442544.15 96426.865 442542.738 96417.06 442536.57 96409.77 442532.05 96400.5 442526.92 96393.44 442524.29 96381.14 442520.15 96374.99 442518.51 96362.59 442515.22 96347.27 442511.56 96334.86 442509.41 96320.29 442507.67 96305.17 442506.12 96290.74 442505.15 96265.42 442504.73 96249.18 442504.2 96237.48 442502.77 96221.208 442500.0 96217.86 442499.43 96205.57 442496.2 96197.57 442493.3 96184.57 442487.8 96169.26 442479.46 96159.16 442472.99 96145.0 442462.48 96133.1 442454.11 96123.812 442448.378 96121.06 442446.68 96109.61 442440.9 96099.45 442436.98 96080.98 442431.34 96071.0 442429.39 96062.58 442428.09 96055.61 442426.45 96039.65 442421.85 96029.3 442419.73 96011.92 442417.61 96006.6 442415.25 96000.0 442410.975 95990.53 442404.84 95983.62 442399.08 95976.06 442390.81 95969.77 442382.58 95964.41 442373.55 95961.61 442365.36 95958.56 442352.79 95958.27 442350.94 95956.43 442339.42 95955.72 442328.42 95955.87 442315.91 95956.27 442304.66 95956.77 442293.57 95953.49 442284.9 95950.79 442278.61 95946.14 442271.09 95940.33 442264.16 95934.5 442259.43 95933.622 442258.759 95921.46 442249.46 95914.85 442245.51 95884.56 442228.58 95877.65 442225.16 95857.44 442215.19 95848.94 442209.93 95840.82 442203.42 95820.58 442185.09 95813.06 442179.09 95806.06 442173.9 95798.06 442167.88 95791.4 442163.37 95784.0 442158.5 95772.02 442150.85 95754.66 442140.27 95730.57 442124.34 95700.79 442104.46 95684.7 442094.55 95668.11 442083.83 95651.612 442074.671 95639.18 442067.77 95616.97 442056.93 95609.36 442054.49 95599.72 442051.52 95590.23 442050.25 95578.21 442049.2 95543.53 442048.13 95533.68 442046.82 95513.95 442042.21 95499.66 442038.1 95484.72 442032.28 95473.61 442027.17 95469.51 442024.28 95464.88 442019.37 95458.97 442012.1 95451.347 442000.0 95448.94 441996.18 95428.31 441957.0 95410.102 441924.209 95390.678 441889.543 95379.701 441869.969 95350.891 441810.145 95344.582 441793.164 95329.406 441754.046 95298.772 441674.585 95298.53 441674.018 95284.013 441686.328 95283.508 441686.756 95276.429 441692.759 95261.511 441705.409 95261.0 441705.842 95255.082 441710.86 95253.701 441712.031 95239.952 441723.69 95239.292 441724.25 95232.948 441729.613 95228.162 441733.687 95220.708 441740.009 95219.845 441740.74 95209.448 441749.557 95166.0 441786.4 95164.7 441786.0 95155.66 441793.12 95100.527 441839.549 95096.89 441842.58 95046.18 441885.29 95000.0 441924.139 94999.983 441924.153 94994.938 441928.397 94979.645 441941.263 94971.9 441936.0 94925.2 441971.73 94900.321 441948.719 94865.67 441916.67 94845.797 441933.13 94838.941 441938.811 94834.6 441942.408 94765.093 442000.0 94671.361 442077.664 94663.979 442083.939 94663.331 442083.177 94533.822 442193.26 94529.724 442196.745 94518.892 442205.951 94516.704 442207.811 94430.0 442281.51 94485.038 442358.735 94484.799 442358.935 94481.851 442361.41 94386.875 442441.167 94370.369 442455.028 94369.751 442455.547 94316.817 442500.0 94285.751 442526.089 94066.679 442710.288 94065.689 442711.128 94003.899 442763.556 94000.0 442766.833 93901.688 442849.468 93897.21 442853.231 93889.815 442859.447 93885.089 442863.419 93827.041 442912.211 93823.411 442915.261 93784.912 442947.621 93784.519 442948.131 93784.383 442948.307 93778.25 442953.482 93771.039 442959.566 93766.556 442963.349 93764.111 442965.412 93754.606 442973.432 93750.259 442977.1 93748.185 442978.849 93730.283 442842.846 93711.592 442739.773 93711.325 442670.653 93711.136 442667.562 93706.616 442535.35 93705.407 442500.0 93705.037 442489.169 93704.902 442485.185 93703.271 442486.54 93699.89 442489.348 93699.533 442489.645 93698.945 442490.133 93687.134 442500.0 93680.479 442505.56 93648.56 442532.226 93645.887 442534.471 93577.829 442591.618 93530.649 442631.192 93529.0 442632.575 93415.714 442727.6 93414.88 442728.3 93205.902 442905.241 93123.48 442974.34 93068.627 443000.0 93062.0 443003.1 93000.0 443055.533 92950.14 443097.7 92950.126 443097.712 92928.141 443116.273 92893.82 443145.25 92888.061 443158.453 92882.33 443171.59 92879.037 443174.282 92867.668 443184.085 92845.479 443202.657 92840.333 443206.677 92797.634 443242.455 92621.741 443389.189 92557.424 443442.794 92552.681 443446.734 92537.968 443458.553 92526.849 443467.376 92506.069 443484.392 92504.336 443485.811 92487.007 443500.0 92483.054 443503.237 92462.232 443520.288 92444.991 443534.406 92438.155 443540.003 92420.009 443554.861 92417.89 443556.597 92406.106 443566.247 92403.22 443568.61 92391.565 443578.077 92346.669 443614.541 92346.524 443614.66 92346.382 443614.775 92327.709 443630.212 92313.415 443642.03 92310.264 443644.637 92292.118 443659.653 92283.554 443666.741 92279.516 443670.144 92264.03 443683.2 92256.743 443689.343 92256.689 443689.388 92256.628 443689.44 92254.49 443691.243 92249.065 443695.658 92243.04 443700.563 92226.011 443714.861 92201.315 443735.598 92195.577 443740.371 92169.959 443761.677 92160.799 443769.295 92119.464 443803.526 92084.382 443833.217 92048.972 443862.861 92024.942 443883.214 92019.118 443888.147 92000.0 443904.212 91983.747 443917.87 91982.818 443918.65 91955.754 443940.708 91953.323 443942.701 91920.523 443969.586 91919.814 443970.176 91916.386 443967.441 91910.284 443962.572 91923.403 443951.519 91933.366 443943.125 91934.837 443941.885 91936.367 443940.596 91949.47 443929.555 91934.616 443908.283 91933.807 443907.124 91930.834 443909.563 91907.838 443879.978 91885.659 443853.464 91861.3 443828.223 91854.785 443822.851 91833.013 443804.201 91810.702 443785.768 91787.899 443767.182 91756.729 443744.04 91697.045 443792.802 91623.723 443750.0 91580.88 443724.99 91569.873 443709.664 91519.07 443638.93 91529.137 443630.825 91548.648 443615.117 91572.27 443596.1 91561.406 443580.077 91561.212 443579.79 91548.053 443560.953 91511.945 443514.011 91511.437 443514.419 91500.779 443500.0 91500.0 443498.946 91486.483 443480.66 91413.493 443380.036 91408.167 443372.789 91385.1 443341.4 91385.871 443336.151 91398.521 443250.0 91400.4 443237.208 91400.9 443233.8 91407.434 443222.091 91411.0 443215.7 91441.4 443103.9 91442.1 443100.8 91440.9 443097.8 91423.99 443081.42 91422.509 443080.256 91418.761 443077.311 91396.818 443060.067 91395.702 443059.19 91389.408 443054.244 91389.129 443054.024 91357.864 443029.456 91339.348 443014.906 91328.865 443006.668 91324.074 443002.903 91320.38 443000.0 91311.882 442993.322 91299.09 442983.27 91317.313 442870.167 91324.86 442823.33 91324.799 442766.869 91324.796 442764.557 91324.79 442758.78 91324.13 442753.83 91312.18 442709.21 91310.19 442704.14 91308.949 442701.89 91308.445 442700.977 91290.88 442669.13 91290.753 442668.899 91286.202 442664.399 91279.246 442657.523 91230.291 442609.125 91203.82 442586.487 91199.605 442582.087 91199.1 442581.56 91197.719 442580.697 91168.91 442562.71 91129.09 442539.48 91097.368 442523.219 91097.355 442523.213 91097.095 442523.08 91089.35 442519.11 91063.689 442508.07 91053.16 442503.54 91043.967 442500.0 91000.0 442483.069 90972.76 442472.58 90885.56 442439.45 90886.82 442437.06 90798.99 442401.37 90768.27 442390.97 90732.86 442378.61 90697.113 442367.841 90653.36 442354.661 90637.079 442349.757 90627.86 442346.98 90607.227 442321.055 90597.49 442308.82 90582.747 442286.319 90558.865 442249.871 90534.974 442213.409 90487.379 442140.771 90416.24 442032.2 90406.175 442044.588 90404.726 442046.372 90402.837 442048.674 90373.653 442024.254 90362.494 442015.44 90360.203 442013.631 90341.443 442036.394 90273.439 442119.747 90227.531 442178.007 90215.611 442194.712 90199.331 442216.35 90153.589 442271.652 90142.728 442284.561 90100.258 442334.676 90089.122 442346.65 90049.224 442391.34 90039.022 442402.866 90001.702 442449.331 90000.0 442451.558 89992.2 442461.764 89962.558 442498.117 89961.018 442500.0 89947.738 442516.239 89912.673 442558.681 89900.866 442573.083 89869.965 442609.558 89857.727 442624.512 89827.638 442661.087 89819.472 442670.61 89793.773 442702.172 89785.544 442712.738 89762.628 442741.141 89751.047 442755.923 89729.224 442782.684 89719.541 442794.641 89697.682 442821.576 89688.293 442833.581 89661.042 442868.215 89653.878 442881.142 89649.49 442889.059 89637.534 442900.521 89628.843 442911.414 89609.881 442935.527 89600.272 442946.568 89594.184 442954.019 89592.717 442956.743 89562.825 442995.114 89559.084 443000.0 89540.326 443024.503 89534.166 443020.45 89533.01 443021.96 89531.81 443023.51 89530.587 443025.276 89530.12 443025.95 89528.49 443028.23 89526.19 443031.39 89523.03 443035.96 89519.21 443041.3 89517.69 443043.45 89516.23 443045.58 89515.04 443047.35 89514.29 443048.62 89513.274 443050.646 89512.83 443051.44 89512.44 443052.22 89512.09 443052.95 89499.067 443069.088 89486.363 443084.833 89481.87 443090.4 89481.411 443090.922 89475.625 443097.78 89473.76 443099.99 89472.604 443101.405 89471.57 443102.67 89463.13 443112.62 89456.99 443119.96 89456.01 443121.31 89455.186 443122.433 89454.88 443122.85 89454.744 443123.084 89454.23 443123.97 89452.86 443126.59 89452.51 443127.23 89452.0 443127.93 89451.52 443128.35 89450.98 443128.76 89450.4 443129.09 89449.74 443129.58 89447.07 443132.94 89446.18 443134.12 89445.1 443135.74 89444.27 443136.92 89444.174 443137.032 89443.3 443138.05 89442.31 443139.18 89440.61 443141.03 89438.82 443143.12 89435.02 443147.51 89427.77 443156.62 89422.23 443163.37 89413.82 443173.59 89411.849 443175.945 89406.98 443181.76 89406.43 443182.58 89405.71 443183.58 89405.05 443184.56 89403.99 443186.21 89403.14 443187.42 89402.6 443187.55 89402.01 443188.35 89400.81 443189.69 89398.4 443192.59 89397.67 443193.64 89397.01 443194.62 89396.06 443196.27 89395.03 443197.99 89393.47 443200.15 89392.343 443201.478 89391.79 443202.13 89390.0 443204.35 89388.57 443205.95 89387.93 443206.54 89387.44 443206.86 89386.62 443207.33 89385.94 443207.67 89385.26 443208.09 89384.4 443208.8 89381.92 443211.37 89378.62 443214.56 89375.14 443218.41 89372.48 443221.47 89362.28 443233.1 89360.505 443235.053 89357.8 443238.03 89352.19 443244.58 89347.081 443250.673 89345.8 443252.2 89339.56 443259.31 89329.58 443271.07 89324.25 443277.58 89319.84 443283.06 89314.96 443289.14 89310.03 443295.51 89305.64 443301.01 89301.43 443306.34 89296.22 443313.31 89293.276 443317.097 89290.42 443320.77 89285.16 443327.49 89279.56 443334.41 89274.52 443340.57 89271.6 443344.69 89269.7 443347.41 89266.87 443350.929 89241.277 443383.737 89234.04 443392.987 89232.1 443395.38 89228.27 443400.13 89225.06 443404.48 89223.114 443407.281 89218.25 443414.39 89215.37 443418.54 89213.27 443421.67 89209.69 443426.14 89206.14 443430.54 89204.03 443433.25 89202.28 443435.36 89200.64 443436.97 89199.01 443438.34 89197.68 443439.636 89197.48 443439.83 89195.58 443442.03 89194.35 443443.62 89193.43 443444.9 89192.45 443446.43 89191.59 443447.79 89188.838 443451.134 89188.24 443451.86 89188.02 443451.75 89183.89 443456.66 89180.67 443460.38 89173.931 443469.417 89168.14 443476.75 89166.56 443478.72 89166.18 443479.25 89165.82 443480.17 89165.39 443480.87 89162.037 443485.823 89153.026 443500.0 89148.259 443507.5 89113.04 443558.649 89105.66 443569.35 89096.291 443559.63 89093.729 443556.972 89089.389 443552.47 89071.172 443533.131 89062.794 443524.237 89030.15 443500.0 89019.247 443491.905 89000.0 443480.424 88954.957 443453.554 88899.41 443414.124 88812.535 443362.464 88726.295 443301.678 88626.615 443237.732 88600.255 443218.271 88542.645 443182.128 88440.345 443128.292 88364.166 443079.523 88265.248 443016.198 88240.355 443000.0 88154.743 442944.291 88055.823 442880.087 88000.0 442843.0 87998.395 442841.934 87941.768 442808.58 87877.736 442763.633 87814.906 442725.683 87749.874 442680.337 87703.775 442650.991 87687.367 442640.489 87678.504 442634.432 87667.952 442627.604 87658.09 442621.382 87649.34 442616.004 87638.324 442609.412 87630.517 442604.126 87623.074 442599.467 87615.323 442594.181 87607.445 442589.697 87599.765 442584.454 87591.212 442579.199 87572.852 442566.951 87557.87 442556.533 87551.862 442552.625 87545.194 442548.306 87539.454 442544.625 87535.895 442542.3 87532.251 442540.347 87528.776 442538.671 87525.4 442537.375 87523.219 442536.409 87520.223 442535.01 87518.141 442534.414 87513.062 442533.632 87506.985 442532.779 87496.266 442530.537 87491.75 442529.262 87488.795 442528.11 87484.631 442526.896 87481.55 442526.012 87479.004 442525.127 87476.514 442524.15 87474.389 442523.184 87472.828 442522.402 87470.45 442521.096 87468.396 442519.626 87465.751 442518.268 87462.98 442516.911 87460.026 442514.813 87457.226 442512.375 87450.67 442507.912 87443.214 442503.13 87436.7 442499.253 87428.609 442493.976 87410.406 442481.574 87407.94 442479.894 87379.583 442460.574 87379.577 442460.57 87377.651 442459.231 87377.456 442459.096 87350.905 442440.651 87323.021 442423.276 87316.784 442419.105 87301.701 442409.017 87261.896 442381.754 87246.674 442370.927 87233.348 442361.779 87208.481 442345.013 87185.567 442330.797 87173.404 442323.405 87142.269 442303.183 87129.472 442294.595 87105.603 442278.74 87084.251 442265.484 87049.624 442243.937 87033.269 442233.527 86974.334 442194.993 86957.011 442184.064 86836.467 442105.515 86790.993 442075.932 86761.182 442058.055 86738.545 442044.64 86738.372 442044.537 86736.064 442043.169 86709.206 442027.252 86693.975 442058.062 86627.023 442208.992 86600.609 442273.063 86588.398 442301.453 86569.774 442295.577 86569.509 442295.493 86568.748 442295.253 86564.26 442293.837 86561.897 442293.092 86556.971 442291.538 86555.074 442290.94 86549.631 442289.548 86549.525 442289.521 86548.048 442289.143 86545.591 442288.515 86463.9 442267.628 86461.79 442267.089 86460.773 442276.777 86462.297 442277.075 86453.156 442326.158 86448.103 442324.865 86402.761 442313.26 86383.979 442308.453 86348.3 442299.321 86232.383 442269.579 86217.613 442266.008 86172.786 442255.17 86000.0 442212.175 85967.541 442204.098 85914.341 442190.869 85906.001 442188.795 85778.181 442157.011 85761.422 442152.655 85760.961 442152.535 85757.92 442151.745 85756.521 442151.381 85750.64 442149.853 85741.768 442147.547 85734.191 442145.578 85732.106 442145.036 85730.998 442144.763</gml:posList>
                </gml:LinearRing>
              </gml:exterior>
            </gml:Polygon>
          </bag_LVC:woonplaatsGeometrie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2020022700000100</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20200227</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.20/1871</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:woonplaatsStatus>Woonplaats aangewezen</bag_LVC:woonplaatsStatus>
        </bag_LVC:Woonplaats>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2020, 02, 27, 00, 00, 01);
            var expectedWoonplaats = new BagWoonplaats(
                id: "3086",
                version: new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                name: "Rotterdam");

            // act
            var woonplaats = BagParser.ParseWoonplaatsen(xmlReader);

            // assert
            woonplaats.Should().ContainSingle();
            woonplaats.Single().Should().BeEquivalentTo(expectedWoonplaats);
        }

        [Fact]
        public void ParseInvalidObject()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Woonplaats>
          <bag_LVC:identificatie>3086</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2020022700000100</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20200227</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.20/1871</bagtype:documentnummer>
          </bag_LVC:bron>
          <bag_LVC:woonplaatsStatus>Woonplaats aangewezen</bag_LVC:woonplaatsStatus>
        </bag_LVC:Woonplaats>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            // act
            var woonplaats = BagParser.ParseWoonplaatsen(xmlReader);

            // assert
            woonplaats.Should().BeEmpty();
        }
    }
}